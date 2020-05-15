﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Normal variables
    GameObject        playerAttack;
    BoxCollider2D     attackHitbox;

    public float      attackTime = 1f;
    public bool       attackFlag = false;

    Animator          playerAnim;
    PlayerMovement    playerMove;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerAnim.SetBool("attackW", attackFlag);

        if (attackFlag)
        {
            attackTime -= Time.deltaTime;
        }

        if ((attackTime <= 0) && (playerAttack != null))
        {
            attackTime = 0.46f;
            attackFlag = false;

            Destroy(playerAttack);
        }

        // Verifies if there is a 'Hitbox' tagged GameObject as a child, and if the animation has stopped 
        // playing, destroying said object afterwards.

        // Verifies if the button is pressed and if there is no animation playing
        // IMP - Need to figure out how to implement the buffer
        // IMP - Need to implement the rest of the attacks
        if (playerMove.IsGrounded())
        {
            if (Input.GetButtonDown("Fire1") && !attackFlag)
            {
                Attack("W", new Vector2(43, 13), new Vector2(-21.6f, 2.2f), new Vector3(48f, 0f, 0));
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
    void Attack(string input, Vector2 size, Vector2 offset, Vector3 lPos)
    {

        Debug.Log("Pressed " + input);

        playerAttack = new GameObject("W_Attack");
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

    }
}
