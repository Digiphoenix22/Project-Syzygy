using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInEffect : MonoBehaviour
{
    public Image fadeInOverlay;
    public float fadeDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float currentTime = 0f;

        // Start with a fully opaque overlay
        fadeInOverlay.color = new Color(fadeInOverlay.color.r, fadeInOverlay.color.g, fadeInOverlay.color.b, 1f);

        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            fadeInOverlay.color = new Color(fadeInOverlay.color.r, fadeInOverlay.color.g, fadeInOverlay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the overlay is completely transparent at the end
        fadeInOverlay.color = new Color(fadeInOverlay.color.r, fadeInOverlay.color.g, fadeInOverlay.color.b, 0f);
        fadeInOverlay.gameObject.SetActive(false); // Optionally deactivate the overlay to avoid blocking clicks
    }
}
