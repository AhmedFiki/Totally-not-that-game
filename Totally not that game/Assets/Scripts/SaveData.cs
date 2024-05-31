using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    //data class that holds all game data for saving and loading
    public int rows;
    public int cardCount;

    public int score;
    public int matches;
    public int turns;

    public List<CardData> cardsData = new List<CardData>();

}
