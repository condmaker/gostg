using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public AudioClip enemyDeathSound;
    public AudioClip lineDestructionSound;
    public Animator enemyAnim;

    public int       enemyHealth;
    public int       premadeHealth;
    public float     invulTimer = 3.0f;
    public bool      isInvul = false;

    public float     deathRestore;
    public Slider    slider;
    public int       healthLine;
    public float     lineTimer = 0.5f;
    public float     deathTimer = 1f;

    private GameObject boss;
    private LinesInEnemy linesInEnemy;
    public bool   onDead;
    private bool   compGet = false;

    void Start()
    {
        StartCoroutine(GetComps());
    }

    void Update()
    {
        if (isInvul)
            invulTimer -= Time.deltaTime;
        if (!compGet)
            return;

        enemyHealth = healthLine * 10;
        //Bugs
        slider.value = enemyHealth;

        if (invulTimer <= 0)
        {
            invulTimer = 3;
            isInvul = false;
        }

        if (onDead)
        {
            gameObject.layer = 12;
            StartCoroutine(DestroyEnemy(gameObject));
        }
    }

    public void DestroyLine(GameObject line, HealthPoints playerHealth)
    {
        if (onDead)
            return;

        if (gameObject.tag == "bossEnemy")
        {
            boss = GameObject.FindObjectOfType<HA_Movement>().gameObject;
            boss.GetComponent<EnemyHealth>().CommonDamage(GameObject.FindObjectOfType<HealthPoints>());
        }

        healthLine--;
        //SoundMng.instance.PlaySound(lineDestructionSound);
        Destroy(line);

        if (healthLine <= 0)
        {
            gameObject.layer = 12;
            healthLine = 0;
            playerHealth.RestoreHealth(deathRestore);
            SoundMng.instance.PlaySound(enemyDeathSound);
            onDead = true;
        }
    }

    public void CommonDamage(HealthPoints playerHealth)
    {
        if (onDead)
            return;

        isInvul = true;

        healthLine--;

        if (healthLine <= 0)
        {
            gameObject.layer = 12;
            healthLine = 0;
            playerHealth.RestoreHealth(deathRestore);
            SoundMng.instance.PlaySound(enemyDeathSound);
            onDead = true;
        }
    }

    IEnumerator DestroyEnemy(GameObject gameObject)
    {
        float enemyDestroyTimer = deathTimer;

        enemyAnim.SetBool("onDead", true);
        yield return new WaitForSeconds(enemyDestroyTimer);

        Destroy(gameObject);
    }

    IEnumerator LineDestruction(GameObject line)
    {
        float lineDestroyTimer = lineTimer;

        Debug.Log("Boom?");
        line.layer = 12;

        //animação de fade da linha
        yield return new WaitForSeconds(lineDestroyTimer);
        

        Debug.Log("Boom!");
        //DestroyLine(line);
    }

    IEnumerator GetComps()
    {
        yield return new WaitForSeconds(0.1f);

        Slider[] sliders = GetComponentsInChildren<Slider>();

        foreach (var s in sliders)
        {
            if (s.name == "SliderEnemy")
            {
                slider = s;
                break;
            }
        }

        enemyAnim = GetComponent<Animator>();
        linesInEnemy = GetComponent<LinesInEnemy>();

        if (!linesInEnemy.hasNotLine)
            healthLine = linesInEnemy.NumOfLines;
        else
            healthLine = premadeHealth;

        // Boss bugs on that, need a slider
        slider.maxValue = healthLine * 10;

        compGet = true;
    }
}
