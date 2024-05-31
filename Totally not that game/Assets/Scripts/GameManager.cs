using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    //layout size
    [SerializeField] private Vector2Int gridSize;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterClick(Card card)
    {
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
            StartCoroutine(ReFlipTimer(firstCard, secondCard, true));
        }
        else
        {//flip again
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

    }

    public void RestartGame()
    {

    }

    public void SetGridSize(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
    }

}
