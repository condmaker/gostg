using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HA_Movement : MonoBehaviour
{
    Rigidbody2D          bossBody;
    EnemyHealth          health;

    private Vector2      moveSpeed;
    private Teleporter[] allTp;

    public int           ballCooldownMax = 10;
    public int           enemyCooldownMax = 20;

    private int          ballCooldown = 10;
    private int          enemyCooldown = 20;
    private int          fullHp;
    private bool         cutscene;

    // Start is called before the first frame update
    void Start()
    {
        bossBody = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        allTp = GameObject.FindObjectsOfType<Teleporter>();

        moveSpeed = new Vector2(20, 0, 0);
        fullHp = health.enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (cutscene) return;

        foreach (Teleporter i in allTp)
        {
            if (i.warp)
                transform.position = new Vector3(i.posOffset);
        }

        if (health.enemyHealth <= (fullHp / 2))
        {
            moveSpeed += moveSpeed;
            //ballCooldownMax -= num
            //enemyCooldownMax -= num
        }

        if (ballCooldown <= 0)
        {
            StartCoroutine(ThrowBall);
        }
        else
        {
            ballCooldown -= Time.deltaTime;
        }

        if (enemyCooldown <= 0)
        {
            StartCoroutine(SpawnEnemy);
        }
        else
        {
            enemyCooldown -= Time.deltaTime;
        }

        bossBody.velocity = moveSpeed;
    }

    IEnumerator ThrowBall()
    {
        ballCooldown = ballCooldownMax;
        //resto
    }

    IEnumerator SpawnEnemy()
    {
        enemyCooldown = enemyCooldownMax;
        Instatiate(Puppet1, transform.position - new Vector2(50, 0));
        //resto
    }
}
