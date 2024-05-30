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


    //card flip ease in
    public AnimationCurve scaleDownCurve; 
    //card flip ease out
    public AnimationCurve scaleUpCurve; 


    [ContextMenu("Flip Card")]
    public void FlipCard()
    {
        StartCoroutine(AnimateCard());
    }

    IEnumerator AnimateCard()
    {
        float elapsedTime = 0f;

        //scale x to 0
        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;
            float curveValue = scaleDownCurve.Evaluate(t);
            transform.localScale = new Vector3(Mathf.Lerp(1, 0, curveValue),transform.localScale.y,transform.localScale.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

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
    }

    public void HideCard()
    {

    }

    public void ChangeImage(Sprite sprite)
    {
        image.sprite=sprite;

    }

}
