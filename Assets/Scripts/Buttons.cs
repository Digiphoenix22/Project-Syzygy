using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class Buttons : MonoBehaviour
{
    public AudioSource musicSource;  // Reference to the AudioSource for music
    public AudioSource sfxSource;    // Reference to the AudioSource for SFX
    public AudioClip menuNoise;  // Used for click sounds
    public AudioClip hoverSound;  // Used for hover sounds
    public AudioClip menuSong;  // Background music
    public GameObject optionsMenuUI; // Options menu
    void Start()
    {
        musicSource.clip = menuSong;  // Set the music clip
        musicSource.loop = true;      // Loop the music
        musicSource.Play();           // Play background music on start
    }
    public void OnMouseOver()
    {
        // Play hover sound using the SFX source
        sfxSource.PlayOneShot(hoverSound);
    }

    public void OnMouseDown()
    {
        // Play click sound using the SFX source
        sfxSource.PlayOneShot(menuNoise);
    }


     public void StartGame()
    {
        // Play click sound effect when starting the game
        sfxSource.PlayOneShot(menuNoise);
        Invoke("StartChange", 0.7f);  // Defer scene loading to allow sound to play
    }

    public void Quit()
    {
        Application.Quit();  // Will work once version is built
    }

    public void Options()
    {
        sfxSource.PlayOneShot(menuNoise);
        optionsMenuUI.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        optionsMenuUI.SetActive(false);

    }


    public void WorldMap()
    {
        sfxSource.PlayOneShot(menuNoise);
        Invoke("MapChange", 0.7f);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Credits()
    {
        sfxSource.PlayOneShot(menuNoise);
        Invoke("CreditsChange", 0.7f);
    }

    private void MapChange()
    {
        SceneManager.LoadScene("WorldMap");
    }

    private void OptionsChange()
    {
        SceneManager.LoadScene("Options Menu");
    }

    private void StartChange()
    {
        SceneManager.LoadScene("Level1-1");
    }

    private void CreditsChange()
    {
        SceneManager.LoadScene("Credits");
    }
}

