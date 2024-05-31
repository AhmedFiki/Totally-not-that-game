using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] int id = 0; //#
    [SerializeField] Image image;
    [SerializeField] public GameObject visuals;
    [SerializeField] public Sprite itemImage; //#
    [SerializeField] Sprite backImage;

    [SerializeField] bool isFlipped = false;
    [SerializeField] bool flipping = false;
    [SerializeField] bool hidden = false; //#

    [SerializeField] float flipDuration = .5f;


    //card flip ease in
    public AnimationCurve scaleDownCurve;
    //card flip ease out
    public AnimationCurve scaleUpCurve;

    private void Start()
    {
        if(hidden)
            HideCard();
    }

    [ContextMenu("Flip Card")]
    public void FlipCard()
    {
        StartCoroutine(AnimateCard());
    }

    IEnumerator AnimateCard()
    {
        flipping = true;
        float elapsedTime = 0f;

        //scale x to 0
        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;
            float curveValue = scaleDownCurve.Evaluate(t);
            transform.localScale = new Vector3(Mathf.Lerp(1, 0, curveValue), transform.localScale.y, transform.localScale.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //change sprite, flip logic
        FlipImage();

        elapsedTime = 0f;

        //scale x to 1
        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;
            float curveValue = scaleUpCurve.Evaluate(t);
            transform.localScale = new Vector3(Mathf.Lerp(0, 1, curveValue), transform.localScale.y, transform.localScale.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        flipping = false;
        //done flipping
    }

    public void HideCard()
    {
        visuals.SetActive(false);
        hidden=true;
    }
    public void FlipImage()
    {
        if (!isFlipped)
        {
            ChangeImage(itemImage);
        }
        else
        {
            ChangeImage(backImage);
        }
        isFlipped = !isFlipped;
    }

    public void OnClick()
    {
        if(!flipping)
        GameManager.Instance.RegisterClick(this);
    }

    public void OnHover()
    {
        image.color = Color.gray;
    }
    public void UnHover()
    {
        image.color= Color.white;
    }

    public void ChangeImage(Sprite sprite)
    {
        image.sprite = sprite;

    }
    public int GetID()
    {
        return id;
    }
    public void SetID(int id)
    {
        this.id = id;
    }
    public bool IsHidden()
    {
        return hidden;
    }
    public void SetHidden(bool h)
    {
        hidden = h; 
    }
    public void SetItemSprite(Sprite sprite)
    {
        itemImage = sprite; 
    }
}
