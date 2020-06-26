using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HA_Movement : MonoBehaviour
{
    Rigidbody2D          bossBody;
    EnemyHealth          health;

    public GameObject    puppet;
    public GameObject    ball;
    private Vector2      moveSpeed;
    public float         moveSpeedIncrement;
    public float         ballIncrement;
    public float         enemyIncrement;

    public float         ballCooldownMax = 10;
    public float         enemyCooldownMax = 20;

    private float        ballCooldown = 10;
    private float        enemyCooldown = 20;
    private float        fullHp;
    private bool         cutscene;

    // Start is called before the first frame update
    void Start()
    {
        bossBody = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();

        moveSpeed = new Vector2(-50f, 0);
        fullHp = health.enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (cutscene) return;

        if (health.enemyHealth <= (fullHp / 2))
        {
            moveSpeed = new Vector2(-moveSpeedIncrement, 0);
            ballCooldownMax = ballIncrement;
            enemyCooldownMax = enemyIncrement;
        }

        if (ballCooldown <= 0)
        {
            ThrowBall();
        }
        else
        {
            ballCooldown -= Time.deltaTime;
        }

        if (enemyCooldown <= 0)
        {
            SpawnEnemy();
        }
        else
        {
            enemyCooldown -= Time.deltaTime;
        }

        bossBody.velocity = moveSpeed;
    }

    public void ThrowBall()
    {
        ballCooldown = ballCooldownMax;
        //resto
        Instantiate(ball, transform.position - new Vector3(0, Random.Range(-50, 50), 0), transform.rotation);
        return;
    }

    public void SpawnEnemy()
    {
        enemyCooldown = enemyCooldownMax;
        Instantiate(puppet, transform.position - new Vector3(50, 0, 0), Quaternion.identity);
        //resto
        return;
    }
}
