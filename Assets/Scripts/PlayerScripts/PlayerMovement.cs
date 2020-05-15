﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables that help determine if the player is grounded or not
    public             Transform groundPoint;
    public             Transform groundPointLeft;
    public             Transform groundPointRight;
    public             LayerMask groundLayer;

    // Defines the normal Force Multiplier
    public float       forceMultiplier = 20.0f;
    public float       airForceMultiplier = 10.0f;
    public float       maxSpeed = 50.0f;
    public float       smallJumpSpeed = 150;
    public float       bigJumpSpeed = 250;

    public Vector3     groundDetectorPos = new Vector3(-24f, -0.8f, 0f);
    public Vector2     currentVelocity;

    public float       hAxis;
    public float       vAxis;

    // Colliders to change on air/ground
    public BoxCollider2D groundUpCollider;
    public BoxCollider2D groundDownCollider;
    public BoxCollider2D airCollider;

    // Defines the playerRigid body to add forces and control them
    public Rigidbody2D playerBody;
    Animator           playerAnim;
    SpriteRenderer     playerSprite;
    PlayerAttack       playerAtk;
    HealthPoints       hp;

    // Flags
    ushort             jumpFlag = 0;
    public bool        doubleJumpState = false;
    public bool        jumpState = false;

    // Start is called before the first frame update
    void Start()
    {
        playerBody =     GetComponent<Rigidbody2D>();
        playerAtk =      GetComponent<PlayerAttack>();
        playerAnim =     GetComponent<Animator>();
        playerSprite =   GetComponent<SpriteRenderer>();

        hp =             GetComponent<HealthPoints>();

        hp.onDead += OnDead;
        hp.onHit += OnHit;

    }

    // Using fixed update because of the heavy reliance on velocity manipulation.
    void FixedUpdate()
    {
        if (hp.hp == 0)
        {
            return;
        }

        if (hp.isInvul)
        {
            return;
        }

        // Defines the Horizontal axis according to player input.
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // Obtains the Movement Vector and calculates the x axis velocity with it
        if (!playerAtk.attackFlag)
        {
            float movementVect = hAxis * forceMultiplier;
            currentVelocity = playerBody.velocity;
            currentVelocity.x = movementVect;
        }
        else
        {
            currentVelocity = new Vector2(0, 0);
        }

        // Verifies at the start of the frame if after jumping the "jump" button is still pressed
        if (jumpState & !Input.GetButton("Jump"))
        {
            jumpState = false;
            Debug.Log("Jump Released");
        }

        // Adds a velocity with the horizontal axis player input.
        if (IsGrounded())
        {
            // Switches to ground-based colliders
            ColliderSwitch(false);

            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
            playerBody.velocity = currentVelocity;

            if (doubleJumpState) doubleJumpState = false;
        }
        else
        {
            // Switches to air-based colliders
            ColliderSwitch(true);

            if (hAxis != 0)
            {
                // Adds the velocity according to horizontal Axis, a multiplier, and the Delta Time.
                currentVelocity.x += hAxis * airForceMultiplier * Time.fixedDeltaTime;
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
                jumpFlag += 1;
            }

            JumpHeight();
        }

        //Debug.Log("H/V Axis:" + hAxis + " " + vAxis);
    }

    /// <summary>
    /// Switches colliders based on if the player is grounded or not
    /// </summary>
    /// <param name="status">Boolean verification. True = On Ground / False = On Air</param>
    void ColliderSwitch(bool status)
    {
        if (status)
        {
            airCollider.enabled = true;
            groundPointLeft.position = groundPoint.position;

            groundDownCollider.enabled = false;
            groundUpCollider.enabled =   false;
        }
        else
        {
            airCollider.enabled = false;
            groundPointLeft.localPosition = groundDetectorPos;

            groundDownCollider.enabled = true;
            groundUpCollider.enabled =   true;
        }
    }

    /// <summary>
    /// Utilizing the jump flags, verifies if the player made a Big or Small jump, and executes it
    /// </summary>
    private void JumpHeight()
    {
        // If the jump flag is equal or higher than 25, the player will perform
        // a high jump. The program then resets the jump flag.
        if (jumpFlag >= 10)
        {
            if (!IsGrounded())
                doubleJumpState = true;

            Debug.Log("Big Jump");

            playerBody.velocity = new Vector2(0, bigJumpSpeed);
            jumpFlag = 0;
            jumpState = true;
        }

        // If the jump flag is equal or higher than 1 and the Up Arrow is NOT
        // pressed, the player will perform a short jump.
        if (jumpFlag >= 1 && !Input.GetButton("Jump"))
        {
            if (!IsGrounded())
                doubleJumpState = true;

            Debug.Log("Small Jump");

            playerBody.velocity = new Vector2(0, smallJumpSpeed);
            jumpFlag = 0;
            jumpState = true;
        }
    }

    private void OnDead()
    {
        Debug.Log("Death.");

        playerBody.velocity = new Vector2(0, 200f); 
    }

    private void OnHit()
    {
        Debug.Log("Hit!");

        // Animation Trigger here
        if (transform.rotation == Quaternion.identity)
            playerBody.velocity = new Vector2(-100f, 100f);

        else if (transform.rotation == Quaternion.Euler(0, 180, 0))
            playerBody.velocity = new Vector2(100f, 100f);
    }

    /// <summary>
    /// Checks if player is grounded or not
    /// </summary>
    /// <returns>Boolean</returns>
    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundPoint.position, 0.1f, groundLayer) != null)      return true;
        if (Physics2D.OverlapCircle(groundPointRight.position, 0.1f, groundLayer) != null) return true;
        if (Physics2D.OverlapCircle(groundPointLeft.position, 0.1f, groundLayer) != null)  return true;

        return false;
    }

    void onDestroySelf()
    {
        Destroy(gameObject);
    }
}