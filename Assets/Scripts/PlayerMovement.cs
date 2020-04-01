using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Defines the playerRigid body to add forces and control them
    Rigidbody2D playerBody;
    // Defines the normal Force Multiplier
    float forceMultiplier = 10.0f;
    // A flag that defines the jump buffer
    int jumpBufferFlag = 0;

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
        //float vAxis = Input.GetAxis("Vertical");
        Vector2 movementVect = new Vector2(hAxis * forceMultiplier, 0);

        // Adds a force with the horizontal axis player input.
        playerBody.AddForce(movementVect);

        // Observes if the Up Arrow is pressed and adds +1 to the flag in
        // each frame.
        if (Input.GetKey(KeyCode.UpArrow))
        {
            jumpBufferFlag += 1;
        }

        // If the jump flag is equal or higher than 22, the player will perform
        // a high jump. The program then resets the jump flag.
        // OBS: Need to do something so that jump does nothing when player has
        // not yet landed.
        if (jumpBufferFlag >= 25)
        {
            playerBody.AddForce(new Vector2(0, 300));

            Debug.Log("Get BIG!");
            Debug.Log(jumpBufferFlag);

            jumpBufferFlag = 0;

        }

        // If the jump flag is equal or higher than 1 and the Up Arrow is NOT
        // pressed, the player will perform a short jump.
        if (jumpBufferFlag >= 1 && !Input.GetKey(KeyCode.UpArrow))
        {
            playerBody.AddForce(new Vector2(0, 200));

            Debug.Log("Get SMALL!");
            Debug.Log(jumpBufferFlag);

            jumpBufferFlag = 0;

        }

        //Debug.Log("Force Multiplier:" + forceMultiplier);
        //Debug.Log("H/V Axis:" + movementVect);
    }
}
