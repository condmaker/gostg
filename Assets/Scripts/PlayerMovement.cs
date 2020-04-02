using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Defines the playerRigid body to add forces and control them
    Rigidbody2D playerBody;
    // Defines the normal Force Multiplier
    float forceMultiplier = 20.0f;
    float airForceMultiplier = 3.0f;
    // A flag that defines the jump buffer
    int jumpBufferFlag = 0;
    //
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Defines the Horizontal axis according to player input.
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        // Obtains the Movement Vector and calculates the x axis velocity with it
        float movementVect = hAxis * forceMultiplier;
        Vector2 currentVelocity = playerBody.velocity;
        currentVelocity.x = movementVect;

        // Adds a force with the horizontal axis player input.
        // Needs to: 
        //*NIMP*    Increase gravity pull so it goes up faster and falls EVEN faster
        //
        //*NIMP*    Make it so that changing directions on the air does not snap to the 
        //          other side (ADDED, needs to put a velocity cap to make it consistent)
        // 
        //*NIMP*    Make it so when player stops on the ground it does not stops IMMEDI-
        //          ATELY. Give it few milisseconds of a "slide"
        //
        //*IMP*     Add functional double jump (w/ flags with the isGrounded)
        if (isGrounded)
            playerBody.velocity = currentVelocity;
        else
        {
            if (hAxis != 0)
            {
                currentVelocity.x *= airForceMultiplier;
                playerBody.AddForce(currentVelocity);
            }
        }

        Debug.Log(isGrounded);

        // Observes if the Up Arrow is pressed and adds +1 to the flag in
        // each frame.
        if (vAxis > 0)
        {
            jumpBufferFlag += 1;
        }

        // If the jump flag is equal or higher than 22, the player will perform
        // a high jump. The program then resets the jump flag.
        // OBS: Need to do something so that jump does nothing when player has
        // not yet landed.
        if (jumpBufferFlag >= 25)
        {
            playerBody.velocity = new Vector2(0, 10);

            Debug.Log("Get BIG!");
            Debug.Log(jumpBufferFlag);

            jumpBufferFlag = 0;

        }

        // If the jump flag is equal or higher than 1 and the Up Arrow is NOT
        // pressed, the player will perform a short jump.
        if (jumpBufferFlag >= 1 && (vAxis == 0))
        {
            playerBody.velocity = new Vector2(0, 5);

            Debug.Log("Get SMALL!");
            Debug.Log(jumpBufferFlag);

            jumpBufferFlag = 0;

        }

        //Debug.Log("Force Multiplier:" + forceMultiplier);
        Debug.Log("H/V Axis:" + hAxis + " " + vAxis);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
