using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGrid : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int columns = 3;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] List<GameObject> cards = new List<GameObject>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();



    private void Start()
    {
        columns = PlayerPrefs.GetInt("columns", 3);
        AdjustCellSize();
        InstantiateGridCards(PlayerPrefs.GetInt("cardcount", 6));

    }
    private void AdjustCellSize()
    {
        RectTransform containerRectTransform = gridLayoutGroup.GetComponent<RectTransform>();
        float cellWidth = (containerRectTransform.rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right) - ((columns - 1) * gridLayoutGroup.spacing.x)) / columns;
        float cellHeight = (containerRectTransform.rect.height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom) - ((columns - 1) * gridLayoutGroup.spacing.y)) / columns;
        float avg = (cellWidth + cellHeight) /2;
        Debug
            .Log(cellWidth+" "+cellHeight+" "+avg);
        gridLayoutGroup.cellSize = new Vector2(avg, avg);
    }

    public void InstantiateGridCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            InstantiateGridCard();
        }
        AssignGridCards();

    }

    void ShuffleSpriteList()
    {
        int n = sprites.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Sprite temp = sprites[i];
            sprites[i] = sprites[j];
            sprites[j] = temp;
        }
    }
    [ContextMenu("Test Grid")]
    public void TestInstantiateGrid()
    {
        InstantiateGridCards(10);
    }

    [ContextMenu("Instantiate card")]
    public void InstantiateGridCard()
    {
        GameObject go = Instantiate(cardPrefab, transform);
        cards.Add(go);
    }

    public void AssignGridCards()
    {
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
        }
    }

    void ShuffleCardList(List<Transform> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
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
    public void SetColumns(int c)
    {
        columns = c;
    }
}
