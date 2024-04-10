using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class Buttons : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip menuNoise; 
    public AudioClip menuSong; 
    
    public void start()
    {
        audioSource.Play();
    }

    public void StartGame()
    {
        audioSource.clip = menuNoise;
        audioSource.Play();

        Invoke("startchange", 0.7f); //eventually change TestScene to an integer variable that represents the furthest completed level
    }

    public void Quit()
    {
        Application.Quit(); //will work once version is built
    }

    public void Options()
    {
        audioSource.clip = menuNoise;
        audioSource.Play();
        
        Invoke("optionschange", 0.7f); 
    }

    public void WorldMap()
    {
        audioSource.clip = menuNoise;
        audioSource.Play();
        //SceneManager.LoadScene("WorldMap");
        Invoke("mapchange", 0.7f); 
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void mapchange()
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void optionschange()
    {
        SceneManager.LoadScene("Options Menu");
    }

    public void startchange()
    {
        SceneManager.LoadScene("TestScene");
    }
}
