﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Normal variables
    public AudioClip     attackSound;

    GameObject           playerAttack;
    GameObject           playerHitbox;
    BoxCollider2D        attackCheck;
    BoxCollider2D        attackHitbox;
    ContactFilter2D      contactFilter;
    ContactFilter2D      attackFilter;
    [SerializeField] LayerMask            enemyMask;
    [SerializeField] LayerMask hitboxMask;
    Rigidbody2D          rb;

    public float         comboStringCounter = 3;
    public float         comboDowntime = 3f;
    public bool          comboEndFlag = false;

    public bool          attackCooldown = false;
    public float         attackCooldownTimer = 1.0f;

    public float         attackTime = 0.40f;
    public float         attackTimeAir = 0.40f;
    public bool          attackFlag = false;
    public bool          attackFlagAir = false;
    public bool          bufferBypass = false;
    public CurrentAttack currentAttack = CurrentAttack.None;
    public CurrentAttack collisionCheck = CurrentAttack.None;
    public GameObject    currentLine;
    public Collider2D    currentLineCol1;
    public Collider2D    currentLineCol2;

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
        rb = GetComponent<Rigidbody2D>();

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

        playerAnim.SetBool("attackQA", currentAttack == CurrentAttack.QAirAttack);
        playerAnim.SetBool("attackWA", currentAttack == CurrentAttack.WAirAttack);
        playerAnim.SetBool("attackEA", currentAttack == CurrentAttack.EAirAttack);
        playerAnim.SetBool("attackRA", currentAttack == CurrentAttack.RAirAttack);

        if (attackFlag)
        {
            if (collisionCheck != 0)
            {
                Debug.Log("Enter colCh");
                CheckLineCollision();
            }
            else
                CheckEnemyCollision();

            attackTime -= Time.deltaTime;
        }
        else if (!attackFlagAir)
            collisionCheck = 0;

        if (attackFlagAir)
        {
            if (collisionCheck != 0)
                CheckLineCollision();
            else
                CheckEnemyCollision();

            if (currentAttack != CurrentAttack.RAirAttack)
                attackTimeAir -= Time.deltaTime;
        }
        else if (!attackFlag)
            collisionCheck = 0;

        if (attackCooldown)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (attackCooldownTimer <= 0)
        {
            attackCooldown = false;
            attackCooldownTimer = 1.0f;
        }

        if (attackTime <= 0)
        {
            attackTime = 0.40f;
            attackFlag = false;
            currentAttack = 0;

            Destroy(playerAttack);
        }

        if (attackTimeAir <= 0)
        {
            attackTimeAir = 0.40f;
            attackFlagAir = false;
            currentAttack = 0;

            Destroy(playerAttack);
        }

        if ((currentAttack == CurrentAttack.RAirAttack) && (playerMove.IsGrounded()))
        {
            Destroy(playerAttack);
            currentAttack = 0;
        }

        // Verifies if there is a 'Hitbox' tagged GameObject as a child, and if the animation has stopped 
        // playing, destroying said object afterwards.

        // Verifies if the button is pressed and if there is no animation playing
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
        else if (!playerMove.IsGrounded() && !playerAnim.GetBool("shikiDeath") && !attackFlagAir)
        {
            // Q Attack
            if (currentAttack != CurrentAttack.QAirAttack)
                BufferAttackAir("Fire2", buttonPressedQ, 0);

            // W Attack
            if (currentAttack != CurrentAttack.WAirAttack)
                BufferAttackAir("Fire1", buttonPressedW, 1);

            // E Attack
            if (currentAttack != CurrentAttack.EAirAttack)
                BufferAttackAir("Fire3", buttonPressedE, 2);

            // R Attack
            if (currentAttack != CurrentAttack.RAirAttack)
                BufferAttackAir("Fire4", buttonPressedR, 3);
        }

        if (comboStringCounter <= 0)
        {
            comboDowntime -= Time.deltaTime;

            if (attackTime <= 0.05f)
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
                buttonPressed = false;
            else
                buttonPressed = true;

            if (attackFlag)
            {
                buttonPressed = false;
                comboStringCounter--;
                bufferBypass = true;
            }
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
                        AttackGround("Q", new Vector2(30, 43.8f), new Vector2(-15f, 21.6f), new Vector3(48f, -30f, 0));
                        break;
                    case 1:
                        AttackGround("W", new Vector2(43, 13), new Vector2(-21.5f, 0.5f), new Vector3(68f, 3f, 0));
                        break;
                    case 2:
                        AttackGround("E", new Vector2(30, 48.4f), new Vector2(-15f, -24.2f), new Vector3(48f, 30f, 0));
                        break;
                    case 3:
                        AttackGround("R", new Vector2(18.2f, 12.8f), new Vector2(-9.28f, 2.59f), new Vector3(44.4f, 5f, 0));
                        break;
                }

                //SoundMng.instance.PlaySound(attackSound, 0.5f);

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

    private void BufferAttackAir(string Fire, bool buttonPressed, ushort attackIdentif)
    {
        if (Input.GetButtonDown(Fire))
        {
            switch (attackIdentif)
            {
                case 0:
                    // Animation TBD
                    AttackAir("Q", new Vector2(30f, 30f), new Vector2(-15f, 18f), new Vector3(48f, -30f, 0));
                    break;
                case 1:
                    AttackAir("W", new Vector2(20.5f, 17.56f), new Vector2(-35.68f, -8f), new Vector3(68f, 3f, 0));
                    break;
                case 2:
                    AttackAir("E", new Vector2(30, 48.4f), new Vector2(-15f, -24.2f), new Vector3(48f, 30f, 0));
                    break;
                case 3:
                    AttackAir("R", new Vector2(17.31f, 44f), new Vector2(-33.56f, -26.4f), new Vector3(44.4f, 5f, 0));
                    break;
            }
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
        
        playerAttack.transform.SetParent(transform);

        // Creates an attack hitbox on playerAttack and plays correct attack animation
        // IMP - Needs to check if player is flipped or not
        attackCheck = playerAttack.AddComponent<BoxCollider2D>();

        if (input != "R")
        {
            playerHitbox = new GameObject(input + "_Hitbox");
            playerHitbox.transform.SetParent(playerAttack.transform);
            attackHitbox = playerHitbox.AddComponent<BoxCollider2D>();
        }
        
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

                attackHitbox.size = new Vector2(21.4f, 21.4f);
                attackHitbox.offset = new Vector2(-19.3f, 32.6f);
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

                attackHitbox.size = new Vector2(11, 16);
                attackHitbox.offset = new Vector2(-24.45f, -39.85f);
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

    private void AttackAir(string input, Vector2 size, Vector2 offset, Vector3 lPos)
    {
        Debug.Log("(Air) Pressed " + input);

        playerAttack = new GameObject(input + "_Attack");
        playerAttack.transform.SetParent(transform);

        // Creates an attack hitbox on playerAttack and plays correct attack animation
        attackCheck = playerAttack.AddComponent<BoxCollider2D>();

        if (input != "R")
        {
            playerHitbox = new GameObject(input + "_Hitbox");
            playerHitbox.transform.SetParent(playerAttack.transform);
            attackHitbox = playerHitbox.AddComponent<BoxCollider2D>();
        }

        attackCheck.size = size;
        attackCheck.offset = offset;

        playerAttack.layer = 9;
        playerAttack.transform.localPosition = lPos;

        Debug.Log("Attack Vect: " + playerAttack.transform.position);

        // Initiates the corresponding animation
        attackFlagAir = true;

        switch (input)
        {
            case "Q":
                currentAttack = CurrentAttack.QAirAttack;

                attackHitbox.size = new Vector2(14, 14);
                attackHitbox.offset = new Vector2(-23f, 25.7f);
                playerHitbox.layer = 15;
                break;
            case "W":
                currentAttack = CurrentAttack.WAirAttack;

                attackHitbox.size = size;
                attackHitbox.offset = offset;
                playerHitbox.layer = 15;
                break;
            case "E":
                currentAttack = CurrentAttack.EAirAttack;

                attackHitbox.size = new Vector2(30, 38.4f);
                attackHitbox.offset = new Vector2(-24.45f, -7.95f);
                playerHitbox.layer = 15;
                break;
            case "R":
                currentAttack = CurrentAttack.RAirAttack;
                playerAttack.layer = 15;
                break;
        }

        // Flips the attack if needed
        if (transform.rotation != Quaternion.identity)
            playerAttack.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void CheckEnemyCollision()
    {
        Collider2D[] results = new Collider2D[5];
        int nCollisions = 0;

        if (attackCheck != null)
            nCollisions = Physics2D.OverlapCollider(attackCheck, contactFilter, results);

        if (nCollisions > 0)
        {
            for (int i = 0; i < nCollisions; i++)
            {
                Collider2D enemyCollider = results[i];

                foreach (Transform t in enemyCollider.transform)
                {
                    GameObject line = t.gameObject;

                    if (((line.tag == "line_dh") || (line.tag == "line_uh")) 
                        && (currentAttack == CurrentAttack.QGroundAttack || currentAttack == CurrentAttack.EGroundAttack
                        || currentAttack == CurrentAttack.QAirAttack || currentAttack == CurrentAttack.EAirAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLine = line;
                        currentLineCol1 = currentLine.GetComponent<Collider2D>();
                        currentLineCol2 = currentLine.GetComponent<Collider2D>();
                        Debug.Log(collisionCheck);
                    }
                    else if (((line.tag == "line_rv") || (line.tag == "line_lv")) 
                        && (currentAttack == CurrentAttack.QCGroundAttack || currentAttack == CurrentAttack.ECGroundAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLine = line;
                        currentLineCol1 = currentLine.GetComponent<Collider2D>();
                        currentLineCol2 = currentLine.GetComponent<Collider2D>();
                        Debug.Log(collisionCheck);
                    }
                    else if ((line.tag == "line_h") 
                        && (currentAttack == CurrentAttack.WGroundAttack || currentAttack == CurrentAttack.WAirAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLine = line;
                        currentLineCol1 = currentLine.GetComponent<Collider2D>();
                        Debug.Log(collisionCheck);
                    }
                    else if ((line.tag == "line_v") 
                        && (currentAttack == CurrentAttack.QCGroundAttack || currentAttack == CurrentAttack.ECGroundAttack
                        || currentAttack == CurrentAttack.RAirAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLine = line;
                        currentLineCol1 = currentLine.GetComponent<Collider2D>();
                        Debug.Log(collisionCheck);
                    }
                    else if (((line.tag == "point") || (line.tag == "lpoint") 
                        && currentAttack == CurrentAttack.RGroundAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLine = line;
                        currentLineCol1 = currentLine.GetComponent<Collider2D>();
                        Debug.Log(collisionCheck);
                    }
                }
            }
        }
    } 

    private void CheckLineCollision()
    {
        Collider2D[] results = new Collider2D[5];
        int nCollisions;

        if ((collisionCheck == CurrentAttack.RGroundAttack) || (collisionCheck == CurrentAttack.RAirAttack))
            nCollisions = Physics2D.OverlapCollider(attackCheck, attackFilter, results);
        else
            nCollisions = Physics2D.OverlapCollider(attackHitbox, attackFilter, results);

        Debug.Log("nCollisions: " + nCollisions);
        if (nCollisions >= 1)
        {
            Debug.Log("testy test2");
            currentLine.transform.parent.GetComponent<EnemyHealth>().DestroyLine(currentLine);

            collisionCheck = 0;
        }
    }
}

