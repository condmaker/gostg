using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    Rigidbody2D      enemyBody;
    public Transform groundPoint;
    public LayerMask groundMask;
    Vector2          direction;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        direction = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction.x >= 0)
        {
            if (transform.right.x < 0)
                transform.rotation = Quaternion.identity;

            if (IsGrounded())
            {
                direction = -direction;
                return;
            }

            enemyBody.velocity = new Vector2(100, 0);
        }
        else
        {
            if (transform.right.x > 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);

            if (IsGrounded())
            {
                direction = -direction;
                return;
            }

            enemyBody.velocity = new Vector2(-100, 0);
        }
    }

    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundPoint.position, 0.1f, groundMask) != null) return true;

        return false;
    }
}
