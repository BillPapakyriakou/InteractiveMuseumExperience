using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{

    public GameObject dialogueUI;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public bool isDialogueShown = false;

    CameraController controller;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();

        //dialogueText.richText = true;

        // Add a link click handler component to the dialogueText
        //dialogueText.gameObject.AddComponent<LinkOpener>();

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && isDialogueShown)
        {
            Cursor.lockState = CursorLockMode.Locked;
            EndDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueShown = true;
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
        StartCoroutine(TypeSequence(sentence, .01f));

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
        isDialogueShown = false;
        //Debug.Log("End of info");
        dialogueUI.SetActive(false);
        //FindObjectOfType<CameraController>().ToggleMovement();
        controller.ToggleMovement();
        Cursor.lockState = CursorLockMode.Locked;
    }



}
