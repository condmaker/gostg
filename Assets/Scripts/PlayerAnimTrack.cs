using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTrack : MonoBehaviour
{
    // Defines the playerRigid body to add forces and control them
    PlayerMovement     playerMove;
    Animator           playerAnim;
    SpriteRenderer     playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMove.IsGrounded())
        {
            // Changes the animations if the player is moving or not, flips sprite on X depending on
            // direction
            if (playerMove.currentVelocity.x > 0)
            {
                playerSprite.flipX = false;
                playerAnim.Play("ShikiRun");
            }

            else if (playerMove.currentVelocity.x < 0)
            {
                playerSprite.flipX = true;
                playerAnim.Play("ShikiRun");
            }

            else playerAnim.Play("ShikiIdle");
        }
    }
}
