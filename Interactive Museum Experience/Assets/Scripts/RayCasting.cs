using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour
{
    public GameObject[] objectsToHit;

    DialogueTrigger trigger;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            foreach (GameObject obj in objectsToHit)
            {
                if (obj == hitObject)
                {

                    if (Input.GetKeyUp(KeyCode.I) && !FindObjectOfType<QuizManager>().quizIsActive && !FindObjectOfType<Puzzle>().puzzleIsActive)
                    {
                        DialogueTrigger trigger = hitObject.GetComponent<DialogueTrigger>();

                        if (trigger != null)
                        {
                            trigger.TriggerDialogue();
                        }

                        //FindObjectOfType<DialogueTrigger>().TriggerDialogue();
                    }
                    //FindObjectOfType<DialogueTrigger>().TriggerDialogue();

                    break;
                }
            }

            
        }
        else
        {
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
            
        }
    }

    
}
