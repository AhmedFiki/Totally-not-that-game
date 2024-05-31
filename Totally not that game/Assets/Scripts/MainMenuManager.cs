using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //rows and columns depending on difficulty
    public int rows = 3
        , columns = 2;

    public Button continueButton;

    private void Start()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable= false;
        }
    }

    public void PlayDifficulty(int difficulty)
    {//change grid size depending on difficulty
        switch (difficulty)
        {
            case 0:
                rows = 2;
                columns = 3;
                StartGame();
                break;

            case 1:
                rows = 3;
                columns = 4;
                StartGame();
                break;

            case 2:
                rows = 4;
                columns = 5;
                StartGame();
                break;

            case 3:
                rows = 6;
                columns = 6;
                StartGame();
                break;

            default:
                rows = 3;
                columns = 4;
                StartGame();
                break;

        }

    }
    public void StartGame()
    {
        //start a new game

        PlayerPrefs.SetInt("rows", rows);
        PlayerPrefs.SetInt("cardcount", rows * columns);

        PlayerPrefs.DeleteKey("SaveData");

        SceneManager.LoadScene(1);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        //continue saved game
        SceneManager.LoadScene(1);
    }

    public void SetRows(int rows)
    {
        this.rows = rows;
    }
    public void SetColumns(int columns)
    {
        this.columns = columns;
    }

}
