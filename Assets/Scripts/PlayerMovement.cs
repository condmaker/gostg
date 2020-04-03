using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public             Transform groundPoint;
    public             LayerMask groundLayer;

    // Defines the playerRigid body to add forces and control them
    Rigidbody2D playerBody;

    // Defines the normal Force Multiplier
    public float       forceMultiplier = 20.0f;
    public float       airForceMultiplier = 10.0f;
    float              maxSpeed = 50.0f;

    // A flag that defines the jump buffer
    ushort             jumpBufferFlag = 0;

    // Bools
    bool               doubleJumpState = false;

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
        //*IMP*    Increase gravity pull so it falls faster
        if (IsGrounded())
        {
            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
            playerBody.velocity = currentVelocity;

            if (doubleJumpState)
            {
                doubleJumpState = false;
            }
        }
        else
        {
            if (hAxis != 0)
            {
                currentVelocity.x += hAxis * airForceMultiplier * Time.deltaTime;
                currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
                playerBody.velocity = currentVelocity;
            }
        }

        // Observes if the Up Arrow is pressed and adds +1 to the flag in
        // each frame.
        //
        // *IMP*    É preciso colocar um tempo para que o jumpBufferFlag itere novamente
        //          após o pulo-- ver como colocar o timer.
        if (!doubleJumpState)
        {
            if (vAxis > 0)
            {
                jumpBufferFlag += 1;
            }

            JumpHeight(vAxis);
        }

        //Debug.Log("H/V Axis:" + hAxis + " " + vAxis);
    }

    void JumpHeight(float vAxis)
    {
        Debug.Log("Entered JState" + doubleJumpState);

        // If the jump flag is equal or higher than 25, the player will perform
        // a high jump. The program then resets the jump flag.
        // OBS: Need to do something so that jump does nothing when player has
        // not yet landed.
        if (jumpBufferFlag >= 25)
        {
            if (!IsGrounded())
                doubleJumpState = true;

            Debug.Log("Big Jump");

            playerBody.velocity = new Vector2(0, 10);
            jumpBufferFlag = 0;
        }

        // If the jump flag is equal or higher than 1 and the Up Arrow is NOT
        // pressed, the player will perform a short jump.
        if (jumpBufferFlag >= 1 && (vAxis == 0))
        {
            if (!IsGrounded())
                doubleJumpState = true;

            Debug.Log("Small Jump");

            playerBody.velocity = new Vector2(0, 5);
            jumpBufferFlag = 0;
        }
    }

    bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundPoint.position, 0.1f, groundLayer) != null) return true;

        return false;
    }
}
