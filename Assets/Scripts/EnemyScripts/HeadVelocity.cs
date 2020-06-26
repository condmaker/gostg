using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadVelocity : MonoBehaviour
{
    private Rigidbody2D rb;
    public int          vel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(vel, 0);
    }
}
