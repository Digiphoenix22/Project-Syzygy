using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    FadeInOut fade;
    private int alignedShipsCount = 0;
    public int totalShipsToWin; // Set this in the inspector based on the level
    public GameObject Player;

    void Awake()
    {
        Player = GameObject.Find("Player");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        fade.FADE_IN();
    }
    void Update()
    {
        if (Player == null)
        {
            Invoke("Reset", 1.5f);
        }
    }

    public void AddAlignedShip()
    {
        alignedShipsCount++;
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (alignedShipsCount >= totalShipsToWin)
        {
            Debug.Log("Win Condition Met!");
            StartCoroutine(LevelComplete());
            // Trigger win state here (e.g., load next level, show win screen, etc.)
        }
    }

    IEnumerator LevelComplete()
    {
        Time.timeScale = 0f;
        fade.FADE_OUT();
        yield return new WaitForSeconds(1);
        Time.timeScale = 1f;
        Debug.Log("LOADSCENE");
        SceneManager.LoadScene("WorldMap");
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}