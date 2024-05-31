using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //current score
    private int score;
    [SerializeField] TextMeshProUGUI scoreText;

    //number of correct matches
    private int matches;
    [SerializeField] TextMeshProUGUI matchesText;

    //number of turns (every 2 presses)
    private int turns;
    [SerializeField] TextMeshProUGUI turnsText;



    //current pressed cards references
    private Card firstCard, secondCard;
    bool first = true;
    [SerializeField] float cardRegisterDuration = .75f;

    [SerializeField] int scorePerMatch = 100;

    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip rightClip;
    public AudioClip wrongClip;
    public AudioClip cardClip;

    [SerializeField] GameObject winPanel;

    int availableCards = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        availableCards = PlayerPrefs.GetInt("cardcount", 12);
    }
    public void RegisterClick(Card card)
    {
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
            AddMatch();
            PlayAudioClip(rightClip);
            StartCoroutine(ReFlipTimer(firstCard, secondCard, true));
        }
        else
        {//flip again
            PlayAudioClip(wrongClip);

            StartCoroutine(ReFlipTimer(firstCard, secondCard, false));
        }

        AddTurn();
    }

    IEnumerator ReFlipTimer(Card first, Card second, bool correct)
    {

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

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = score.ToString();
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

    public void RestartGame()
    {

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
        SceneManager.LoadScene(1);

    }
    public void SaveQuit()
    {

    }
}
