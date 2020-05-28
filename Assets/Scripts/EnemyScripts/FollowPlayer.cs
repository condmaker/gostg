using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Collider2D playerDetector;

    Rigidbody2D enemyBody;
    Animator enemyAnim;

    ContactFilter2D contactFilter;
    public LayerMask playerMask;

    public Transform groundPoint;
    public LayerMask groundMask;

    public bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        playerDetector = gameObject.transform.Find("Player Detector").GetComponent<Collider2D>();
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        enemyAnim = gameObject.GetComponent<Animator>();

        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(playerMask);
        contactFilter.useTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnim.SetBool("enemyRunning", isRunning);

        Collider2D[] results = new Collider2D[1];

        int nCollisions = Physics2D.OverlapCollider(playerDetector, contactFilter, results);

        if (nCollisions > 0)
        {
            Vector2 direction = results[0].gameObject.transform.position - transform.position;
            direction.Normalize();

            if (direction.x >= 0)
            {
                transform.rotation = Quaternion.identity;

                if (!IsGrounded())
                    return;

                enemyBody.velocity = new Vector2(30, 0);
                isRunning = true;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

                if (!IsGrounded())
                    return;

                enemyBody.velocity = new Vector2(-30, 0);
                isRunning = true;
            }

        }
        else
            isRunning = false;
    }

    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundPoint.position, 0.1f, groundMask) != null) return true;

        return false;
    }
}
