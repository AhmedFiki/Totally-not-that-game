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


    [SerializeField] GameObject cardPrefab;

    //current pressed cards references
    private Card firstCard, secondCard;
    //layout size
    [SerializeField] private Vector2Int gridSize;


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

    public void SetGridSize(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
    }

}
