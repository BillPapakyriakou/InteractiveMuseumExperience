using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour
{

    DialogueTrigger trigger;
    public LayerMask mask;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100, mask))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            //Debug.Log("We hit a " + hitInfo.transform.name);
            if (Input.GetKeyUp(KeyCode.I)) 
            {
                FindObjectOfType<DialogueTrigger>().TriggerDialogue();
            }
            //FindObjectOfType<DialogueTrigger>().TriggerDialogue();
        }
        else
        {
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
            
        }
    }

    
}
