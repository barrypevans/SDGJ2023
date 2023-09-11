using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TitleSplash : MonoBehaviour
{
    float waitSeconds = 2;
    CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowText(float secs = 2)
    {
        waitSeconds = secs;
        StartCoroutine(Co_ShowText());
    }

    IEnumerator Co_ShowText()
    {
        while (canvasGroup.alpha < .99)
        {
            canvasGroup.alpha += .1f;
            yield return new WaitForSeconds(.1f);
        }
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(waitSeconds);
        while (canvasGroup.alpha > .01)
        {
            canvasGroup.alpha -= .1f;
            yield return new WaitForSeconds(.1f);
        }
        canvasGroup.alpha = 0;
    }
}
