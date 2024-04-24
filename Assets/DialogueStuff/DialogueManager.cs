using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public UnityEngine.UI.Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    public Button ContinueButton;

    public Button Option1Button;
    public Button Option2Button;

    public TextMeshProUGUI Option1Text;
    public TextMeshProUGUI Option2Text;

    private Queue<DialogueLine> lines;
    private List<DialogueLine> LINES;

    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    private int lineIndex = 0;
    public Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance == null) 
        Instance = this;

        lines = new Queue<DialogueLine>();
        LINES = new List<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        gameObject.SetActive(true);
        //isDialogueActive = true;

        animator.Play("show");

        lines.Clear();

        foreach(DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
            LINES.Add(dialogueLine);
        }
        
        NewDisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));

        Debug.Log(lines.Count);
    }

    public void NewDisplayNextDialogueLine()
    {
        if (lineIndex > LINES.Count - 1)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = LINES[lineIndex];

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        if(currentLine.dialogueOptions.Count > 0 )
        {
            DisplayOptions(currentLine.dialogueOptions[0].option, currentLine.dialogueOptions[1].option);
        }
        else
        {
            DisplayContinue();
            lineIndex = currentLine.DefaultDialogueIndex;
        }
        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));

    }

    public void SetNextDialogueViaOptions(int whichOption)
    {
        if(whichOption == 0) {lineIndex = LINES[lineIndex].dialogueOptions[0].dialogueIndex; }
        else { lineIndex = LINES[lineIndex].dialogueOptions[1].dialogueIndex; }
        NewDisplayNextDialogueLine();
    }

    public void DisplayOptions(string option1, string option2)
    {
        Option1Button.gameObject.SetActive(true);
        Option2Button.gameObject.SetActive(true);
        ContinueButton.gameObject.SetActive(false);
        Option1Text.text = option1;
        Option2Text.text = option2;
    }

    public void DisplayContinue()
    {
        Option1Button.gameObject.SetActive(false);
        Option2Button.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(true);
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        //isDialogueActive = false;
        animator.Play("hide");
        GameEvents.dialogueComplete();
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
