using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CardGrid : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int rows = 3;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] List<GameObject> cards = new List<GameObject>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();


    private void Start()
    {
        //handle grid instantiation
        rows = PlayerPrefs.GetInt("rows", 3);
        AdjustCellSize();
        InstantiateGridCards(PlayerPrefs.GetInt("cardcount", 12));

        StartCoroutine(InitFlip());


    }
    void CheckAvailableCards()
    {
        int unHiddenCards = 0;

        foreach (var card in cards)
        {
            if (!card.GetComponent<Card>().IsHidden())
            {
                unHiddenCards++;
            }
        }
        GameManager.Instance.availableCards = unHiddenCards;
    }
    IEnumerator InitFlip()
    {
        //handles initial card flip
        foreach (var card in cards)
        {
            card.GetComponent<Card>().FlipCard();
        }
        yield return new WaitForSeconds(3f);
        foreach (var card in cards)
        {
            card.GetComponent<Card>().FlipCard();
        }

    }

    private void AdjustCellSize()
    {
        //calculates grid size from gridlayoutgroup
        RectTransform containerRectTransform = gridLayoutGroup.GetComponent<RectTransform>();
        float cellWidth = (containerRectTransform.rect.height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom) - ((rows - 1) * gridLayoutGroup.spacing.x)) / rows;

        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth);
    }

    public void InstantiateGridCards(int amount)
    {
        //instantiate a number of grid cards
        for (int i = 0; i < amount; i++)
        {
            InstantiateGridCard();
        }
        if(PlayerPrefs.HasKey("SaveData") )
        {
            LoadGridCards();
        }
        else
        {
            AssignGridCards();
        }
        CheckAvailableCards();
    }

    void ShuffleSpriteList()
    {
        int n = sprites.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Sprite temp = sprites[i];
            sprites[i] = sprites[j];
            sprites[j] = temp;
        }
    }

    [ContextMenu("Instantiate card")]
    public void InstantiateGridCard()
    {
        GameObject go = Instantiate(cardPrefab, transform);
        cards.Add(go);
    }

    public void AssignGridCards()
    {
        //loops over cards to assign unique ids and sprites

        ShuffleSpriteList();
        int index = 0;
        for (int i = 0; i < cards.Count; i += 2)
        {
            cards[i].GetComponent<Card>().SetItemSprite(sprites[index]);
            cards[i].GetComponent<Card>().SetID(index);

            cards[i + 1].GetComponent<Card>().SetItemSprite(sprites[index]);
            cards[i + 1].GetComponent<Card>().SetID(index++);
        }
        ShuffleGridChildren();
    }
    public void LoadGridCards()
    {
        //loads previous grid data from save
        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        for (int i = 0; i < cards.Count; i ++)
        {
            cards[i].GetComponent<Card>().SetItemSprite(GetSpriteFromID( GetSpriteID(data.cardsData[i].spriteName)));
            cards[i].GetComponent<Card>().SetID(data.cardsData[i].id);
            cards[i].GetComponent<Card>().SetHidden(data.cardsData[i].hidden);
            GameManager.Instance.unshuffledCards.Add(cards[i]);

        }

    }
    void ShuffleGridChildren()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in gridLayoutGroup.transform)
        {
            children.Add(child);
        }
        ShuffleCardList(children);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
            GameManager.Instance.unshuffledCards.Add(children[i].gameObject);

        }
    }

    void ShuffleCardList(List<Transform> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Transform temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
    private void OnValidate()
    {
        if (gridLayoutGroup != null)
        {
            AdjustCellSize();
        }
    }
    public void SetRows(int r)
    {
        rows = r;
    }

    public int GetSpriteID(string s)
    {
        //expression to extract ID from sprite name (allows serializing card data and its sprite)
        Match match = Regex.Match(s, @"^\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        return -1;
    }
    public Sprite GetSpriteFromID(int id)
    {

        foreach(Sprite sprite in sprites)
        {
            if (GetSpriteID(sprite.name) == id)
            {
                return sprite;
            }
        }
        return null;
    }
}
