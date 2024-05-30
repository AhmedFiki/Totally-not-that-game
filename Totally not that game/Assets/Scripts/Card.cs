using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite frontImage;
    [SerializeField] Sprite backImage;

    [SerializeField] bool isFlipped = false;
    [SerializeField] float flipDuration = 1;

    public void FlipCard()
    {

    }

    public void HideCard()
    {

    }
}
