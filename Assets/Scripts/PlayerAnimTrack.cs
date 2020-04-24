using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTrack : MonoBehaviour
{
    // TODO - Remove this whole script and implement animations directly on the needed scripts

    // Defines the playerRigid body to add forces and control them
    PlayerMovement     playerMove;
    Animator           playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerAnim = GetComponent<Animator>();
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
                if (transform.right.x < 0)
                transform.rotation = Quaternion.identity;
                playerAnim.Play("ShikiRun");
            }

            else if (playerMove.currentVelocity.x < 0)
            {
                if (transform.right.x > 0)
                    transform.rotation = transform.rotation = Quaternion.Euler(0, 180, 0);
                playerAnim.Play("ShikiRun");
            }

            else playerAnim.Play("ShikiIdle");
        }
        else
        {
            // RE-DO Jump
            playerAnim.Play("ShikiJump");
        }
    }
}
