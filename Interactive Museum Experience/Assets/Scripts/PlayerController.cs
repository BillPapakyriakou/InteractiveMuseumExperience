using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // default player pos == x : 1.48, y : 0, z : -1.48

    public CharacterController characterController;

    public float speed = 12f;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool playerDisabled = false;



    void Update()
    {
        if (!playerDisabled)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }


            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            characterController.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);  // Dy = 1/2 * g * t^2

        }
    }

        

    public void TeleportToGalleryOne()
    {
        StartCoroutine(TeleporterOne());
    }

    public void TeleportToGalleryTwo()
    {
        StartCoroutine(TeleporterTwo());
    }

    IEnumerator TeleporterOne()
    {
        playerDisabled = true;
        yield return new WaitForSeconds(.2f);
        gameObject.transform.position = new Vector3(1.48f, 0f, -11.59f);
        yield return new WaitForSeconds(1f);
        playerDisabled = false;
    }

    IEnumerator TeleporterTwo()
    {
        playerDisabled = true;
        yield return new WaitForSeconds(.2f);
        gameObject.transform.position = new Vector3(148f, 5f, -26f);
        yield return new WaitForSeconds(1f);
        playerDisabled = false;
    }
}

    

