using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public CanvasGroup canvasgroup;
    public bool fadein = false;
    public bool fadeout = false;

    public float TimeToFade;
    

    // Update is called once per frame
    void Start()
    {
        GameEvents.onLevelComplete += FADE_OUT;
        FADE_IN();
    }

    public void FADE_IN()
    {
        StartCoroutine(Fading(true));
    }
    IEnumerator Fading(bool fadeIn)
    {
        if(fadeIn)
        {
            while (canvasgroup.alpha >= 0)
            {
                yield return null;
                canvasgroup.alpha -= Time.unscaledDeltaTime / 4;
            }   
        }
        else
        {
            while (canvasgroup.alpha <= 1)
            {
                yield return null;
                canvasgroup.alpha += Time.unscaledDeltaTime * 4;
            }   
        }


    }
    public void FADE_OUT()
    {
        StartCoroutine(Fading(false));
    }
}
