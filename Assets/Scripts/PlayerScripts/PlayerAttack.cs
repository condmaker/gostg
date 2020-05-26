﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Normal variables
    GameObject           playerAttack;
    BoxCollider2D        attackHitbox;

    public float         comboStringCounter = 3;
    public float         comboDowntime = 3f;
    public bool          comboEndFlag = false;

    public bool          attackCooldown = false;
    public float         attackCooldownTimer = 1.0f;

    public float         attackTime = 0.60f;
    public bool          attackFlag = false;
    public bool          bufferBypass = false;
    public CurrentAttack currentAttack = CurrentAttack.None;
    
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
            if (currentAttack != CurrentAttack.RGroundAttack)
                BufferAttackGround("Fire3", buttonPressedR, 2);

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
                        AttackGround("W", new Vector2(43, 13), new Vector2(-21.6f, 2.2f), new Vector3(48f, 0f, 0));
                        break;
                    case 2:
                        AttackGround("E", new Vector2(43, 13), new Vector2(-21.6f, 2.2f), new Vector3(48f, 0f, 0));
                        break;
                    case 3:
                        AttackGround("R", new Vector2(43, 13), new Vector2(-21.6f, 2.2f), new Vector3(48f, 0f, 0));
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
        playerAttack.transform.SetParent(transform);

        // Creates an attack hitbox on playerAttack and plays correct attack animation
        // IMP - Needs to check if player is flipped or not
        attackHitbox = playerAttack.AddComponent<BoxCollider2D>();
        attackHitbox.size = size;
        attackHitbox.offset = offset;

        playerAttack.tag = "Hitbox";
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
                break;
            case "W":
                currentAttack = CurrentAttack.WGroundAttack;
                break;
            case "E":
                currentAttack = CurrentAttack.EGroundAttack;
                break;
            case "R":
                currentAttack = CurrentAttack.RGroundAttack;
                break;
            case "QC":
                currentAttack = CurrentAttack.QCGroundAttack;
                break;
            case "EC":
                currentAttack = CurrentAttack.ECGroundAttack;
                break;
        }
    }
}

