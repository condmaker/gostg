using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables that help determine if the player is grounded or not
    public             Transform groundPoint;
    public             Transform groundPointLeft;
    public             Transform groundPointRight;
    public             LayerMask groundLayer;

    // Defines the playerRigid body to add forces and control them
    Rigidbody2D        playerBody;
    Animator           playerAnim;
    SpriteRenderer     playerSprite;  

    // Defines the normal Force Multiplier
    public float       forceMultiplier = 20.0f;
    public float       airForceMultiplier = 10.0f;
    public float       maxSpeed = 50.0f;
    public float       smallJumpSpeed = 150;
    public float       bigJumpSpeed = 250;

    public Vector2     currentVelocity;

    public float       hAxis;
    public float       vAxis;

    // Flags
    ushort             jumpBufferFlag = 0;
    bool               doubleJumpState = false;
    bool               jumpState = false;

    // Colliders to change on air/ground
    public CapsuleCollider2D groundCollider;
    public BoxCollider2D airCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerBody =   GetComponent<Rigidbody2D>();
        playerAnim =   GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

        groundCollider = GetComponent<CapsuleCollider2D>();
        airCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Defines the Horizontal axis according to player input.
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // Obtains the Movement Vector and calculates the x axis velocity with it
        float movementVect = hAxis * forceMultiplier;
        currentVelocity = playerBody.velocity;
        currentVelocity.x = movementVect;

        // Verifies at the start of the frame if after jumping the "jump" button is still pressed
        if (jumpState & !Input.GetButton("Jump"))
        {
            jumpState = false;
            Debug.Log("Jump Released");
        }

   
        // Adds a velocity with the horizontal axis player input.
        if (IsGrounded())
        {
            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
            playerBody.velocity = currentVelocity;

            if (doubleJumpState) doubleJumpState = false;
        }
        else
        {
            if (hAxis != 0)
            {
                // Adds the velocity according to horizontal Axis, a multiplier, and the Delta Time.
                currentVelocity.x += hAxis * airForceMultiplier * Time.deltaTime;
                // Limits the velocity so that the player does not fly away at high velocities.
                currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);

                if (vAxis < 0)
                    currentVelocity.x = 0;

                playerBody.velocity = currentVelocity;
            }

            // If the player presses down, it stops him immediately on the air.
            if (vAxis < 0)
            {
                currentVelocity.x = 0;
                playerBody.velocity = currentVelocity;
            }
        }

        // Observes if the Jump Button is pressed and adds +1 to the flag in
        // each frame.
        if (!doubleJumpState && !jumpState)
        {
            if (Input.GetButton("Jump"))
            {
                jumpBufferFlag += 1;
            }

            JumpHeight(vAxis);
        }

        //Debug.Log("H/V Axis:" + hAxis + " " + vAxis);
    }

    void JumpHeight(float vAxis)
    {
        //Debug.Log("Entered D.J. State: " + doubleJumpState);

        // If the jump flag is equal or higher than 25, the player will perform
        // a high jump. The program then resets the jump flag.
        if (jumpBufferFlag >= 30)
        {
            if (!IsGrounded())
                doubleJumpState = true;

            Debug.Log("Big Jump");

            playerBody.velocity = new Vector2(0, bigJumpSpeed);
            jumpBufferFlag = 0;
            jumpState = true;
        }

        // If the jump flag is equal or higher than 1 and the Up Arrow is NOT
        // pressed, the player will perform a short jump.
        if (jumpBufferFlag >= 1 && !Input.GetButton("Jump"))
        {
            if (!IsGrounded())
                doubleJumpState = true;

            Debug.Log("Small Jump");

            playerBody.velocity = new Vector2(0, smallJumpSpeed);
            jumpBufferFlag = 0;
            jumpState = true;
        }
    }

    // Checks if player is grounded or not
    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundPoint.position, 0.1f, groundLayer) != null)      return true;
        if (Physics2D.OverlapCircle(groundPointRight.position, 0.1f, groundLayer) != null) return true;
        if (Physics2D.OverlapCircle(groundPointLeft.position, 0.1f, groundLayer) != null)  return true;

        return false;
    }
}
