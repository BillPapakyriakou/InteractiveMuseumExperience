using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public GameObject dialogueUI;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    CameraController controller;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        controller = FindObjectOfType<CameraController>();

        controller.DisableCameraMovement();
        Cursor.lockState = CursorLockMode.None;


        dialogueUI.SetActive(true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }    

        string sentence = sentences.Dequeue();
        
        StopAllCoroutines(); // stops TypeSequence animation if we press continue before  finished
        StartCoroutine(TypeSequence(sentence, .02f));

    }

    IEnumerator TypeSequence(string sentence, float delay)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text  += letter;
            yield return new WaitForSeconds(delay);
        }
    }

    public void EndDialogue()
    {
        //Debug.Log("End of info");
        dialogueUI.SetActive(false);
        //FindObjectOfType<CameraController>().ToggleMovement();
        controller.ToggleMovement();
        Cursor.lockState = CursorLockMode.Locked;
    }
   
}
