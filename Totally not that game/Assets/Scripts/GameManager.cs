using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //current score
    private int score;
    //number of correct matches
    private int matches;
    //number of turns (every 2 presses)
    private int turns;


    public GameObject cardPrefab;

    //current pressed cards references
    public Card firstCard, secondCard;
    //layout size
    public Vector2Int gridSize;


    public void CreateCardGrid()
    {

    }

    public void AddScore(int score)
    {
        this.score += score;
    }
    public void AddMatch()
    {
        this.matches++;
    }
    public void AddTurn()
    {
        this.turns++;
    }

    public void Win()
    {

    }

    public void RestartGame()
    {

    }


}
