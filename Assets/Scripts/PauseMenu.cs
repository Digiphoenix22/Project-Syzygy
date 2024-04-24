using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Main pause menu
    public GameObject optionsMenuUI; // Options menu

    public Image fadeOverlay;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    private bool isFading = false;

    void Start()
    {
        // Initialize sliders with current mixer settings
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenuUI.activeInHierarchy)
            {
                CloseOptionsMenu();  // First close options if it's open
            }
            else if (pauseMenuUI.activeInHierarchy)
            {
                Resume();  // If pause menu is open, close it
            }
            else
            {
                Pause();  // Open pause menu if no other menu is open
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMainMenu()
    {
        if (!isFading)
        {
            StartCoroutine(FadeToMainMenu());
        }
    }

    public void OpenOptionsMenu()
    {
        optionsMenuUI.SetActive(true);
        
    }

    public void CloseOptionsMenu()
    {
        optionsMenuUI.SetActive(false);

    }

    IEnumerator FadeToMainMenu()
    {
        Time.timeScale = 1f;
        isFading = true;
        float fadeDuration = 0.5f;
        float currentTime = 0f;
        fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, 0f);

        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration);
            fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, alpha);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        SceneManager.LoadScene("Main Menu");
    }
}
