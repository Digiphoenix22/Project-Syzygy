using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioMixerGroup musicMixerGroup;

    private AudioSource audioSource;
    public AudioClip level1_1Music;  // For Level 1-1
    public AudioClip level1_2Music;  // For Level 1-2
    public AudioClip level1_3Music;  // For Level 1-3
    public AudioClip mainMenuMusic;  // For the Main Menu

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Makes this object persistent between scenes
            InitializeMusic();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void InitializeMusic()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = musicMixerGroup;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Level1-1":
                audioSource.clip = level1_1Music;
                break;
            case "Level1-2":
                audioSource.clip = level1_2Music;
                break;
            case "Level1-3":
                audioSource.clip = level1_3Music;
                break;
            case "Main Menu":
                audioSource.clip = mainMenuMusic;
                break;
            default:
                audioSource.clip = null;  // No music for unspecified scenes
                break;
        }

        if (audioSource.clip != null)
            audioSource.Play();
        else
            audioSource.Stop();
    }
}
