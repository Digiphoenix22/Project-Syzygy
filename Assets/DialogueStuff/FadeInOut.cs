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
        StartCoroutine(Fading(-0.05f, 0));
    }
    IEnumerator Fading(float fadeVal, float goalVal)
    {
        while(canvasgroup.alpha != goalVal)
        {
            canvasgroup.alpha += fadeVal;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void FADE_OUT()
    {
        StartCoroutine(Fading(0.05f, 1));
    }
}
