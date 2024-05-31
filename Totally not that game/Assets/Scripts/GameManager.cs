using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public const string saveKey = "SaveData";

    public CardGrid cardGrid;
    //current score
    [SerializeField] private int score;
    [SerializeField] TextMeshProUGUI scoreText;

    //number of correct matches
    private int matches;
    [SerializeField] TextMeshProUGUI matchesText;

    //number of turns (every 2 presses)
    private int turns;
    [SerializeField] TextMeshProUGUI turnsText;

    [SerializeField] int combo = 1;
    [SerializeField] TextMeshProUGUI comboText;


    //current pressed cards references
    private Card firstCard, secondCard;
    bool first = true;
    [SerializeField] float cardRegisterDuration = .75f;

    [SerializeField] int scorePerMatch = 100;

    //audio references
    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip rightClip;
    public AudioClip wrongClip;
    public AudioClip cardClip;

    [SerializeField] GameObject winPanel;

    public int availableCards = 0;

    public List<GameObject> unshuffledCards = new List<GameObject>();



    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.HasKey(saveKey))
        {
            //load game if player pressed continue
            LoadGame();
        }


    }
    private void Start()
    {
        //availableCards = PlayerPrefs.GetInt("cardcount", 12);
    }
    public void RegisterClick(Card card)
    {
        //called on any card press
        PlayAudioClip(cardClip);
        card.FlipCard();
        if (first)
        {
            firstCard = card;
            first = false;
        }
        else
        {
            secondCard = card;
            first = true;
            CheckCards();
        }
    }

    public void CheckCards()
    {
        if (firstCard.GetID() == secondCard.GetID())
        {//add score, remove gameobjects
            AddScore(scorePerMatch);
            AddCombo();
            AddMatch();
            PlayAudioClip(rightClip);
            StartCoroutine(ReFlipTimer(firstCard, secondCard, true));
        }
        else
        {//flip again
            PlayAudioClip(wrongClip);
            ResetCombo();
            StartCoroutine(ReFlipTimer(firstCard, secondCard, false));
        }

        AddTurn();
    }

    IEnumerator ReFlipTimer(Card first, Card second, bool correct)
    {
        //coroutine to handle the second pressed card

        yield return new WaitForSeconds(cardRegisterDuration);

        if (!correct)
        {
            first.FlipCard();
            second.FlipCard();
        }
        else
        {
            first.HideCard();
            second.HideCard();
            availableCards -= 2;
        }
        if (availableCards <= 0)
        {
            Win();
        }
    }

    public void AddCombo()
    {
        combo++;
        comboText.text = combo.ToString();
    }
    public void ResetCombo()
    {
        combo = 1; 
        comboText.text = combo.ToString();
    }

    public void AddScore(int score)
    {
        this.score += score*combo;
        scoreText.text = this.score.ToString();
    }
    public void AddMatch()
    {
        this.matches++;
        matchesText.text = matches.ToString();
    }
    public void AddTurn()
    {
        this.turns++;
        turnsText.text = turns.ToString();
    }

    public void Win()
    {
        PlayAudioClip(winClip);
        winPanel.SetActive(true);
    }


    public void PlayAudioClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("No AudioClip!");
            return;
        }

        audioSource.PlayOneShot(clip);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);

    }
    public void Restart()
    {
        PlayerPrefs.DeleteKey("SaveData");

        SceneManager.LoadScene(1);

    }
    public void SaveQuit()
    {
        //save game data and quit to main menu
        SaveData saveData = new SaveData();


        saveData.rows = PlayerPrefs.GetInt("rows", 3);
        saveData.cardCount = PlayerPrefs.GetInt("cardcount", 12);
        saveData.score = this.score;
        saveData.turns = this.turns;
        saveData.matches = this.matches;

        saveData.cardsData = GetCardData();

        SaveGame(saveData);
        SceneManager.LoadScene(0);

    }
 
    public List<CardData> GetCardData()
    {
        //helper function to create a serializable list of card data for saving
        List<CardData> cardDatas = new List<CardData>();

        foreach (var card in unshuffledCards)
        {
            CardData cardData = new CardData();
            Card c = card.GetComponent<Card>();

            cardData.id = c.GetID();
            cardData.spriteName = c.itemImage.name;
            cardData.hidden = c.IsHidden();

            cardDatas.Add(cardData);
        }

        return cardDatas;

    }
    public void SaveGame(SaveData data)
    {
        //save serialized data as json string
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        //loads game variables and stats
        if (PlayerPrefs.HasKey(saveKey))
        {
            string json = PlayerPrefs.GetString(saveKey);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            score = data.score;
            matches = data.matches;
            turns = data.turns;
            DisplayTexts();

            PlayerPrefs.SetInt("rows", data.rows);
            PlayerPrefs.SetInt("cardcount",data.cardCount);

        }
        else
        {
            Debug.LogWarning("No save data found!");
        }
    }

    void DisplayTexts()
    {
        scoreText.text = score.ToString();
        matchesText.text = matches.ToString();
        turnsText.text = turns.ToString();
        
    }
}
