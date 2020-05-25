using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables that help determine if the player is grounded or not
    [SerializeField]             Transform groundPoint;
    [SerializeField]             Transform groundPointLeft;
    [SerializeField]             Transform groundPointRight;
    [SerializeField]             LayerMask groundLayer;

    // Defines the normal Force Multiplier
    [SerializeField] float       forceMultiplier = 20.0f;
    [SerializeField] float       airForceMultiplier = 10.0f;
    [SerializeField] float       maxSpeed = 50.0f;
    [SerializeField] float       smallJumpSpeed = 150;
    [SerializeField] float       bigJumpSpeed = 250;
    [SerializeField] float       coyoteTime = 0.1f;

    [SerializeField] Vector3     groundDetectorPos = new Vector3(-24f, -0.8f, 0f);
    public Vector2               currentVelocity;

    [SerializeField] float       hAxis;
    [SerializeField] float       vAxis;

    // Colliders to change on air/ground
    [SerializeField] BoxCollider2D groundUpCollider;
    [SerializeField] BoxCollider2D groundDownCollider;
    [SerializeField] BoxCollider2D airCollider;

    // Defines the playerRigid body to add forces and control them
    [SerializeField] Rigidbody2D playerBody;
    Animator                     playerAnim;
    SpriteRenderer               playerSprite;
    PlayerAttack                 playerAtk;
    HealthPoints                 hp;

    // Flags
    ushort                       jumpFlag = 0;
    public bool                  doubleJumpState = false;
    [SerializeField] bool        jumpState = false;

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

        // Obtains the Movement Vector and calculates the x axis velocity with it
        if (!playerAtk.attackFlag)
        {
            // Defines the Horizontal axis according to player input.
            hAxis = Input.GetAxis("Horizontal");
            vAxis = Input.GetAxis("Vertical");

            float movementVect = hAxis * forceMultiplier;
            currentVelocity = playerBody.velocity;
            currentVelocity.x = movementVect;
        }
        else
        {
            // Breaks the buffer system
            //Input.ResetInputAxes("Horizontal");
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
            coyoteTime = 0.1f;

            // Switches to ground-based colliders
            ColliderSwitch(false);

            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
            playerBody.velocity = currentVelocity;

            if (doubleJumpState) doubleJumpState = false;
        }
        else if (!IsGrounded() && coyoteTime >= 0)
        {
            coyoteTime -= Time.fixedDeltaTime;

            // Switches to ground-based colliders
            ColliderSwitch(false);

            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);
            playerBody.velocity = currentVelocity;

            if (doubleJumpState) doubleJumpState = false;
        }

        if (coyoteTime < 0)
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

            // Switches velocity when character is falling
            if (currentVelocity.y <= 0)
            {
                playerBody.gravityScale = 100;
            }
            else
            {
                playerBody.gravityScale = 50;
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

            if (!playerAtk.attackFlag) JumpHeight();
        }

        //Debug.Log("H/V Axis:" + hAxis + " " + vAxis);
    }

    /// <summary>
    /// Switches colliders based on if the player is grounded or not
    /// </summary>
    /// <param name="status">Boolean verification. False = On Ground / True = On Air</param>
    void ColliderSwitch(bool status)
    {
        if (status)
        {
            airCollider.enabled = true;
            groundPointLeft.localPosition = groundDetectorPos + new Vector3(17, 0, 0); ;

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

    /// <summary>
    /// Event that triggers when player dies. Needs an death animation.
    /// </summary>
    private void OnDead()
    {
        Debug.Log("Death.");

        gameObject.layer = 12;
        playerAnim.SetBool("shikiDeath", true);
    }

    /// <summary>
    /// Event that triggers when player gets hit. Needs an hit animation.
    /// </summary>
    private void OnHit()
    {
        Debug.Log("Hit!");

        // Animation Trigger here
        playerAnim.SetTrigger("shikiHit");

        if (currentVelocity.x == 0)
        {
            playerBody.velocity = new Vector2(0, 400f);
        }
        else
        {
            if (transform.rotation == Quaternion.identity)
                playerBody.velocity = new Vector2(-100f, 250f);

            else if (transform.rotation == Quaternion.Euler(0, -180, 0))
                playerBody.velocity = new Vector2(100f, 250f);

        }
        
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
