using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int rows;
    public int cardCount;

    public int score;
    public int matches;
    public int turns;

    public List<CardData> cardsData = new List<CardData>();

}
