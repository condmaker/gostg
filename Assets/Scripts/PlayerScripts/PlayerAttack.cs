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
    BoxCollider2D[]      sepColliders;
    [SerializeField] LayerMask            enemyMask;
    [SerializeField] LayerMask hitboxMask;
    Rigidbody2D          rb;

    public float         comboStringCounter;
    public float         comboDowntime;
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

    public List<GameObject>    currentLines;

    [System.Serializable]
    public class AttackButton
    {
        public CurrentAttack attack;

        public string    text;
        public bool      buttonPressed;
        public float     attackHoldTime;
        public ushort    identif;
    }

    public AttackButton[] attackButtons;

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
            for (int i = 0; i < attackButtons.Length; i++)
            {
                if (currentAttack != attackButtons[i].attack)
                {
                    BufferAttackGround(attackButtons[i].text, ref attackButtons[i].buttonPressed, ref attackButtons[i].attackHoldTime, attackButtons[i].identif);
                }
            }

        }     
        else if (!playerMove.IsGrounded() && !playerAnim.GetBool("shikiDeath") && !attackFlagAir)
        {
            // Bug where ground attack gets air buffered and "gets out" when player returns to the ground
            for (int i = 0; i < attackButtons.Length; i++)
            {
                if (currentAttack != attackButtons[i].attack)
                {
                    BufferAttackAir(attackButtons[i].text, attackButtons[i].identif);
                }
            }
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
                comboDowntime = 0.4f;
            }
        }

        if (!attackFlag && (comboStringCounter > 0))
        {
            comboStringCounter = 3;
            comboEndFlag = false;
        }

    }

    private void BufferAttackGround(string Fire, ref bool buttonPressed, ref float currentAttackHold, ushort attackIdentif)
    {
        float attackHoldTime = currentAttackHold;

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

                SoundMng.instance.PlaySound(attackSound, 0.3f);

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

        currentAttackHold = attackHoldTime;
    }

    private void BufferAttackAir(string Fire, ushort attackIdentif)
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

            SoundMng.instance.PlaySound(attackSound, 0.3f);
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
        currentLines.Clear();

        BoxCollider2D[] results = new BoxCollider2D[5];
        int nCollisions = 0;

        if (attackCheck != null)
            nCollisions = Physics2D.OverlapCollider(attackCheck, contactFilter, results);

        if (nCollisions > 0)
        {
            for (int i = 0; i < nCollisions; i++)
            {
                BoxCollider2D enemyCollider = results[i];

                if (enemyCollider.transform.gameObject.tag == "Araya")
                {
                    if (!enemyCollider.transform.gameObject.GetComponent<DamageZone>().enabled)
                        enemyCollider.transform.gameObject.GetComponent<EnemyHealth>().CommonDamage(GetComponent<HealthPoints>());

                    Destroy(attackCheck);
                    return;
                }

                foreach (Transform t in enemyCollider.transform)
                {
                    GameObject line = t.gameObject;

                    // fix
                    if (((line.tag == "line_dh") || (line.tag == "line_uh"))
                        && (currentAttack == CurrentAttack.QGroundAttack || currentAttack == CurrentAttack.QAirAttack 
                             || currentAttack == CurrentAttack.EGroundAttack || currentAttack == CurrentAttack.EAirAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLines.Add(line);
                        sepColliders = line.GetComponents<BoxCollider2D>();
                        Debug.Log("Q, E:" + collisionCheck);
                    } 
                    else if ((line.tag == "line_h") 
                        && (currentAttack == CurrentAttack.WGroundAttack || currentAttack == CurrentAttack.WAirAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLines.Add(line);
                        Debug.Log("W: " + collisionCheck);
                    }
                    else if ((line.tag == "line_v") 
                        && (currentAttack == CurrentAttack.QCGroundAttack || currentAttack == CurrentAttack.ECGroundAttack
                        || currentAttack == CurrentAttack.RAirAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLines.Add(line);
                        Debug.Log("Rair: " + collisionCheck);
                    }
                    else if (((line.tag == "point") || (line.tag == "lpoint"))
                        && (currentAttack == CurrentAttack.RGroundAttack))
                    {
                        collisionCheck = currentAttack;
                        currentLines.Add(line);
                        Debug.Log("R: " + collisionCheck);
                    }
                }
            }
        }
    } 

    private void CheckLineCollision()
    {
        bool test = false;

        Debug.Log("number: " + currentLines.Count);

        foreach (GameObject i in currentLines)
        { 
            BoxCollider2D[] results = new BoxCollider2D[5];
            int nCollisions;

            if (attackCheck == null)
                return;
            else if ((collisionCheck == CurrentAttack.RGroundAttack) || (collisionCheck == CurrentAttack.RAirAttack))
                nCollisions = Physics2D.OverlapCollider(attackCheck, attackFilter, results);
            else
            {
                if (attackHitbox == null)
                    return;

                nCollisions = Physics2D.OverlapCollider(attackHitbox, attackFilter, results);
            }

            Debug.Log("nCollisions: " + nCollisions);
            if (nCollisions >= 1)
            {
                test = false;
                if (currentAttack == CurrentAttack.QGroundAttack || currentAttack == CurrentAttack.QAirAttack)
                {
                    foreach (BoxCollider2D col in results)
                    {
                        if (col != null)
                        {
                            if ((col == sepColliders[0]) && (col.tag == i.tag))
                                test = true;
                        }
                    }

                    if (!test)
                        continue;
                }
                else if (currentAttack == CurrentAttack.EGroundAttack || currentAttack == CurrentAttack.EAirAttack)
                {
                    foreach (BoxCollider2D col in results)
                    {
                        if (col != null)
                        {
                            if ((col == sepColliders[1]) && (col.tag == i.tag))
                                test = true;
                        }
                    }

                    if (!test)
                        continue;
                }

                i.transform.parent.GetComponent<EnemyHealth>().DestroyLine(i, transform.gameObject.GetComponent<HealthPoints>());
                collisionCheck = 0;
            }
        }

        currentLines.Clear();

        if (!test)
            collisionCheck = 0;
    }
}

