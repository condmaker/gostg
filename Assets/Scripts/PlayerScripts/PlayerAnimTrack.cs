using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTrack : MonoBehaviour
{
    public AudioClip walkSound;

    // Defines the playerRigid body to add forces and control them

    PlayerMovement     playerMove;
    PlayerAttack       playerAtk;
    Animator           playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerAnim = GetComponent<Animator>();
        playerAtk = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {

        playerAnim.SetBool("isGrounded", playerMove.IsGrounded());
        playerAnim.SetBool("doubleJumpState", playerMove.doubleJumpState);
        playerAnim.SetFloat("velY", playerMove.currentVelocity.y);
        playerAnim.SetFloat("velX", playerMove.currentVelocity.x);

        if (playerMove.IsGrounded())
        {
            // Changes the animations if the player is moving or not, flips sprite on X depending on
            // direction
            if (!playerAnim.GetBool("shikiDeath"))
            {
                if ((playerMove.currentVelocity.x > 0) && !playerAtk.attackFlag)
                {
                    if (transform.right.x < 0)
                        transform.rotation = Quaternion.identity;

                    playerAnim.Play("ShikiRun");
                    //SoundMng.instance.PlaySound(walkSound, 0.2f);

                }

                else if ((playerMove.currentVelocity.x < 0) && !playerAtk.attackFlag)
                {
                    if (transform.right.x > 0)
                        transform.rotation = Quaternion.Euler(0, 180, 0);

                    playerAnim.Play("ShikiRun");
                    //SoundMng.instance.PlaySound(walkSound, 0.2f);
                }

                else
                {
                    if (!playerAtk.attackFlag) playerAnim.Play("ShikiIdle");
                }
            }
        }
    }
}
