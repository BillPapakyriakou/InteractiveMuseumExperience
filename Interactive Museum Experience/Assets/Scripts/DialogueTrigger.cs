using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (!FindObjectOfType<PauseMenu>().gameIsPaused)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
        
    }

}
