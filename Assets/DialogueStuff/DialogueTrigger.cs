using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    FadeInOut fade;

    [SerializeField] private bool activeOnStart;
    [SerializeField] private float delayTime;

    public void TriggerDialogue()
    {
        if(!activeOnStart)
        {
             GameEvents.onDialogueComplete += nextLevelAfterDialogue;
        }
        StartCoroutine(delayDialogue(delayTime));
    }

    private void Start()
    {
        fade = GetComponent<FadeInOut>();
        if(activeOnStart) TriggerDialogue();
        else 
        {
            GameEvents.onLevelComplete += TriggerDialogue;
        }
    }

    
    public void nextLevelAfterDialogue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator delayDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogueManager.Instance.StartDialogue(dialogue);
    }
    
}
