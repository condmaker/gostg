using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HA_Movement : MonoBehaviour
{
    Rigidbody2D          bossBody;
    EnemyHealth          health;

    private GameObject   puppet;
    private Vector2      moveSpeed;
    private Teleporter[] allTp;

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
        allTp = GameObject.FindObjectsOfType<Teleporter>();

        moveSpeed = new Vector2(20, 0);
        fullHp = health.enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (cutscene) return;

        foreach (Teleporter i in allTp)
        {
            if (i.warp)
                transform.position += i.posOffset;
        }

        if (health.enemyHealth <= (fullHp / 2))
        {
            moveSpeed += moveSpeed;
            //ballCooldownMax -= num
            //enemyCooldownMax -= num
        }

        if (ballCooldown <= 0)
        {
            StartCoroutine("ThrowBall");
        }
        else
        {
            ballCooldown -= Time.deltaTime;
        }

        if (enemyCooldown <= 0)
        {
            StartCoroutine("SpawnEnemy");
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
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator SpawnEnemy()
    {
        enemyCooldown = enemyCooldownMax;
        Instantiate(puppet, transform.position - new Vector3(50, 0, 0), transform.rotation);
        //resto
        yield return new WaitForSeconds(0.1f);
    }
}
