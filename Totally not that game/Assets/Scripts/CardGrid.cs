using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGrid : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int columns = 3;
    [SerializeField] GameObject cardPrefab;

    List<GameObject> cards = new List<GameObject>();
    List<Sprite> sprites = new List<Sprite>();
    private void Start()
    {
        AdjustCellSize();
    }
    private void AdjustCellSize()
    {
        RectTransform containerRectTransform = gridLayoutGroup.GetComponent<RectTransform>();
        float cellWidth = (containerRectTransform.rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right) - ((columns - 1) * gridLayoutGroup.spacing.x)) / columns;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellWidth);
    }

    public void InstantiateGridCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            InstantiateGridCard();
        }


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
            cards[i].GetComponent<Card>().SetID(index++);
            
            cards[i + 1].GetComponent<Card>().SetItemSprite(sprites[index]);
            cards[i+1].GetComponent<Card>().SetID(index++);
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
