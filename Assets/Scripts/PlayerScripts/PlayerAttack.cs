using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Normal variables
    GameObject           playerAttack;
    GameObject           playerHitbox;
    BoxCollider2D        attackCheck;
    BoxCollider2D        attackHitbox;
    ContactFilter2D      contactFilter;
    ContactFilter2D      attackFilter;
    [SerializeField] LayerMask            enemyMask;
    [SerializeField] LayerMask hitboxMask;

    public float         comboStringCounter = 3;
    public float         comboDowntime = 3f;
    public bool          comboEndFlag = false;

    public bool          attackCooldown = false;
    public float         attackCooldownTimer = 1.0f;

    public float         attackTime = 0.60f;
    public bool          attackFlag = false;
    public bool          bufferBypass = false;
    public CurrentAttack currentAttack = CurrentAttack.None;
    public CurrentAttack collisionCheck = CurrentAttack.None;
    public GameObject    currentLine;
    public Collider2D    currentLineCol;
    
    public bool          buttonPressedW = true;
    public float         attackHoldTimeW = 0.5f;
    public bool          buttonPressedQ = true;
    public float         attackHoldTimeQ = 0.5f;
    public bool          buttonPressedR = true;
    public float         attackHoldTimeR = 0.5f;
    public bool          buttonPressedE = true;
    public float         attackHoldTimeE = 0.5f;


    Animator             playerAnim;
    PlayerMovement       playerMove;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerAnim = GetComponent<Animator>();

        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(enemyMask);
        contactFilter.useTriggers = true;

        attackFilter = new ContactFilter2D();
        attackFilter.SetLayerMask(hitboxMask);
        attackFilter.useTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Bools for all possible attacks
        playerAnim.SetBool("attackQ",  currentAttack == CurrentAttack.QGroundAttack);
        playerAnim.SetBool("attackW",  currentAttack == CurrentAttack.WGroundAttack);
        playerAnim.SetBool("attackE",  currentAttack == CurrentAttack.EGroundAttack);
        playerAnim.SetBool("attackR",  currentAttack == CurrentAttack.RGroundAttack);
        playerAnim.SetBool("attackQC", currentAttack == CurrentAttack.QCGroundAttack);
        playerAnim.SetBool("attackEC", currentAttack == CurrentAttack.ECGroundAttack);

        if (attackFlag)
        {
            if (collisionCheck != 0)
            {
                CheckLineCollision();
            }
            else
                CheckEnemyCollision();
            attackTime -= Time.deltaTime;
        }

        if (attackCooldown)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (attackCooldownTimer <= 0)
        {
            attackCooldown = false;
            attackCooldownTimer = 1.0f;
        }

        if ((attackTime <= 0) && (playerAttack != null))
        {
            attackTime = 0.68f;
            attackFlag = false;
            currentAttack = 0;

            Destroy(playerAttack);
        }

        // Verifies if there is a 'Hitbox' tagged GameObject as a child, and if the animation has stopped 
        // playing, destroying said object afterwards.

        // Verifies if the button is pressed and if there is no animation playing
        // IMP - Need to figure out how to implement the buffer (Now to implement universal comboStringCounter)
        // IMP - Need to implement the rest of the attacks
        if (playerMove.IsGrounded() && !playerAnim.GetBool("shikiDeath") && comboStringCounter > 0)
        {
            // Q Attack
            if (currentAttack != CurrentAttack.QGroundAttack)
                BufferAttackGround("Fire2", buttonPressedQ, 0);

            // W Attack
            if (currentAttack != CurrentAttack.WGroundAttack)
                BufferAttackGround("Fire1", buttonPressedW, 1);

            // E Attack
            if (currentAttack != CurrentAttack.EGroundAttack)
                BufferAttackGround("Fire3", buttonPressedE, 2);

            // R Attack
            if (currentAttack != CurrentAttack.RGroundAttack)
                BufferAttackGround("Fire4", buttonPressedR, 3);
        }     

        if (comboStringCounter <= 0)
        {
            comboDowntime -= Time.deltaTime;

            if (attackTime <= 0.15f)
                comboEndFlag = true;

            if (comboDowntime <= 0)
            {
                comboEndFlag = false;
                comboStringCounter = 3;
                comboDowntime = 3f;
            }
        }

        if (!attackFlag && (comboStringCounter > 0))
        {
            comboStringCounter = 3;
            comboEndFlag = false;
        }

    }

    private void BufferAttackGround(string Fire, bool buttonPressed, ushort attackIdentif)
    {
        float attackHoldTime = 0.5f;

        switch (attackIdentif)
        {
            case 0:
                attackHoldTime = attackHoldTimeQ;
                break;
            case 1:
                attackHoldTime = attackHoldTimeW;
                break;
            case 2:
                attackHoldTime = attackHoldTimeE;
                break;
            case 3:
                attackHoldTime = attackHoldTimeR;
                break;
        }

        if (Input.GetButtonUp(Fire))
        {
            if (!attackCooldown)
            {
                Debug.Log("WHY DOE");
                buttonPressed = false;
            }
            else
            {
                Debug.Log("THERE IS NO WAY");
                buttonPressed = true;
            }

            if (attackFlag)
            {
                buttonPressed = false;
                comboStringCounter--;
                bufferBypass = true;
            }
                
            Debug.Log("Giant Dog");
        }

        if (Input.GetButton(Fire))
        {
            if (!buttonPressed) buttonPressed = true;

            if (buttonPressed) attackHoldTime -= Time.deltaTime;
        }

        if (!attackFlag && !comboEndFlag && (!attackCooldown || bufferBypass))
        {
            if ((attackHoldTime > 0f) && (attackHoldTime < 0.5f) && (!buttonPressed || bufferBypass))
            {
                switch (attackIdentif)
                {
                    case 0:
                        AttackGround("Q", new Vector2(30, 34), new Vector2(-15f, 17f), new Vector3(48f, -30f, 0));
                        break;
                    case 1:
                        AttackGround("W", new Vector2(43, 13), new Vector2(-21.5f, 0.5f), new Vector3(68f, 3f, 0));
                        break;
                    case 2:
                        AttackGround("E", new Vector2(30, 48.4f), new Vector2(-15f, -24.2f), new Vector3(48f, 30f, 0));
                        break;
                    case 3:
                        AttackGround("R", new Vector2(12.75f, 10.9f), new Vector2(-6.5f, 0.5f), new Vector3(44.4f, 5f, 0));
                        break;
                }

                if (comboStringCounter == 1)
                    comboStringCounter = 0;

                bufferBypass = false;
                attackHoldTime = 0.5f;
            }
            else if (attackHoldTime <= 0f && !buttonPressed)
            {
                Debug.Log("Hold!");
                attackHoldTime = 0.5f;
                //AttackGround("Heavy",) com switch
            }
            else if (attackHoldTime <= 0f)
            {
                attackHoldTime = 0.5f;
            }

        }

        switch (attackIdentif)
        {
            case 0:
                attackHoldTimeQ = attackHoldTime;
                break;
            case 1:
                attackHoldTimeW = attackHoldTime;
                break;
            case 2:
                attackHoldTimeE = attackHoldTime;
                break;
            case 3:
                attackHoldTimeR = attackHoldTime;
                break;
        }
    }

    /// <summary>
    /// Creates a GameObject in the 'Hitbox' layer that will create the attack and play the animation.
    /// </summary>
    /// <param name="input">The button the player has pressed</param>
    /// <param name="size">Size of the attack hitbox</param>
    /// <param name="offset">Offset of the attack hitbox</param>
    /// <param name="lPos">Position of object's Vector3 in relation to the player</param>
    private void AttackGround(string input, Vector2 size, Vector2 offset, Vector3 lPos)
    {
        Debug.Log("Pressed " + input);

        playerAttack = new GameObject(input + "_Attack");
        playerHitbox = new GameObject(input + "_Hitbox");
        playerAttack.transform.SetParent(transform);
        playerHitbox.transform.SetParent(playerAttack.transform);

        // Creates an attack hitbox on playerAttack and plays correct attack animation
        // IMP - Needs to check if player is flipped or not
        attackCheck = playerAttack.AddComponent<BoxCollider2D>();
        attackHitbox = playerHitbox.AddComponent<BoxCollider2D>();
        playerHitbox.AddComponent<Rigidbody2D>();

        attackCheck.size = size;
        attackCheck.offset = offset;

        playerAttack.layer = 9;
        playerAttack.transform.localPosition = lPos;

        Debug.Log("Attack Vect: " + playerAttack.transform.position);

        // Initiates the corresponding animation
        attackFlag = true;
        attackCooldown = true;

        switch (input)
        {
            case "Q":
                currentAttack = CurrentAttack.QGroundAttack;

                attackHitbox.size = size - new Vector2(10, 10);
                attackHitbox.offset = offset - new Vector2(-24.7f, 28.4f);
                playerHitbox.layer = 15;
                break;
            case "W":                
                currentAttack = CurrentAttack.WGroundAttack;

                attackHitbox.size = size;
                attackHitbox.offset = offset; 
                playerHitbox.layer = 15;
                break;
            case "E":
                currentAttack = CurrentAttack.EGroundAttack;

                attackHitbox.size = size;
                attackHitbox.offset = offset;
                playerHitbox.layer = 15;
                break;
            case "R":
                currentAttack = CurrentAttack.RGroundAttack;
                playerAttack.layer = 15;
                break;
            case "QC":
                currentAttack = CurrentAttack.QCGroundAttack;
                break;
            case "EC":
                currentAttack = CurrentAttack.ECGroundAttack;
                break;
        }

        // Flips the attack if needed
        if (transform.rotation != Quaternion.identity)
            playerAttack.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void CheckEnemyCollision()
    {
        Collider2D[] results = new Collider2D[5];

        int nCollisions = Physics2D.OverlapCollider(attackCheck, contactFilter, results);

        if (nCollisions > 0)
        {
            for (int i = 0; i < nCollisions; i++)
            {
                Collider2D enemyCollider = results[i];

                foreach (Transform t in enemyCollider.transform)
                {
                    GameObject line = t.gameObject;

                    if (((line.tag == "line_dh") || (line.tag == "line_uh")) 
                        && (currentAttack == CurrentAttack.QGroundAttack || currentAttack == CurrentAttack.EGroundAttack))
                    {
                        if (enemyCollider.IsTouchingLayers(15))
                        {
                            Destroy(line);
                        }
                    }
                    else if (((line.tag == "line_rv") || (line.tag == "line_lv")) 
                        && (currentAttack == CurrentAttack.QCGroundAttack || currentAttack == CurrentAttack.ECGroundAttack))
                    {
                        if (enemyCollider.IsTouchingLayers(15))
                        {
                            Destroy(line);
                        }
                    }
                    else if ((line.tag == "line_h") 
                        && currentAttack == CurrentAttack.WGroundAttack)
                    {
                        collisionCheck = currentAttack;
                        currentLine = line;
                        currentLineCol = currentLine.GetComponent<Collider2D>();
                    }
                    else if ((line.tag == "line_v") 
                        && (currentAttack == CurrentAttack.QCGroundAttack || currentAttack == CurrentAttack.ECGroundAttack))
                    {
                        if (enemyCollider.IsTouchingLayers(15))
                        {
                            Destroy(line);
                        }
                    }
                    else if (((line.tag == "point") || (line.tag == "lpoint") 
                        && currentAttack == CurrentAttack.RGroundAttack))
                    {
                        if (enemyCollider.IsTouchingLayers(15))
                        {
                            Destroy(line);
                        }
                    }
                }
            }
        }
    }

    private void CheckLineCollision()
    {
        Collider2D[] results = new Collider2D[5];

        int nCollisions = Physics2D.OverlapCollider(currentLineCol, attackFilter, results);

        switch (collisionCheck)
        {
            case CurrentAttack.WGroundAttack:
                Debug.Log("testy test");
                if (nCollisions > 1)
                {
                    Debug.Log("testy test2");
                    Destroy(currentLine);
                }
                break;
            default:
                break;
        }

        collisionCheck = 0;
    }
}

