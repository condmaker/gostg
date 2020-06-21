using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // TO-DO:
    // Merge this into only one hp script shared with player (not really quintessential)
    // Make player recover health when an enemy dies
    public AudioClip enemyDeathSound;
    public AudioClip lineDestructionSound;
    public Animator enemyAnim;

    public int       enemyHealth;

    public float     deathRestore;
    public Slider    slider;
    public int       healthLine;
    public float     lineTimer = 0.5f;
    public float     deathTimer = 1f;

    private LinesInEnemy linesInEnemy;
    private bool  onDead;

    void OnEnable()
    {
        StartCoroutine(GetComps());
    }

    void Update()
    {
        enemyHealth = healthLine * 10;
        slider.value = enemyHealth;

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

        healthLine--;
        //SoundMng.instance.PlaySound(lineDestructionSound);
        Destroy(line);
        //StartCoroutine(LineDestruction(line));

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

        Debug.Log("Deathy");
        enemyAnim.SetBool("onDead", true);
        yield return new WaitForSeconds(enemyDestroyTimer);
        Debug.Log("Deathy Death");

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
        healthLine = linesInEnemy.NumOfLines;
        slider.maxValue = healthLine * 10;
    }
}
