using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Needed for IEnumerator
using UnityEngine.UI; // Include the UI namespace
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign this in the inspector
    public Image fadeOverlay; // Assign this in the inspector
    private bool isFading = false; // To control the fade effect
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeInHierarchy)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
    }

    public void LoadMainMenu()
    {
        if (!isFading)
        {
            StartCoroutine(FadeToMainMenu());
        }
    }

    IEnumerator FadeToMainMenu()
    {
    // Make sure to resume time when initiating the fade effect
    Time.timeScale = 1f;
    isFading = true;

    float fadeDuration = 0.5f;
    float currentTime = 0f;

    // Ensure the overlay is completely opaque before starting the fade effect
    // in case Time.timeScale was causing issues with coroutine execution.
    fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, 0f);

    // Gradually change the alpha value of the fade overlay from 0 to 1
    while (currentTime < fadeDuration)
    {
        float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration);
        fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, alpha);
        currentTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime for operations during paused state
        yield return null; // Wait for the next frame
    }

    // Load the Main Menu scene after the fade effect completes
    SceneManager.LoadScene("Main Menu");
    }

    

    // Add an Options function if you have an options menu
    public void LoadOptions()
    {
        Debug.Log("Load options menu");
        // Implement options menu functionality here
    }
}
