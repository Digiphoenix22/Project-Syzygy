using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    // Start is called before the first frame update
    private void Awake()
    {
        current = this;
    }

    public static event Action onLevelComplete;
    public static event Action onDialogueComplete;
    public static void levelComplete()
    {
        onLevelComplete?.Invoke();
    }

    public static void dialogueComplete()
    {
        onDialogueComplete?.Invoke();
    }
}
