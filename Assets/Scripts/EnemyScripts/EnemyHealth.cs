using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int    enemyHealth;

    public Slider slider;
    public int    healthLine;
    public float  lineTimer = 0.5f;

    private LinesInEnemy linesInEnemy;
    private bool  onDead;

    void Start()
    {
        slider = transform.Find("Canvas").transform.Find("SliderEnemy").GetComponent<Slider>();
        linesInEnemy = GetComponent<LinesInEnemy>();
        healthLine = linesInEnemy.NumOfLines;
        slider.maxValue = healthLine * 10;
    }

    void Update()
    {
        enemyHealth = healthLine * 10;
        slider.value = enemyHealth;

        if (onDead)
            Destroy(gameObject);
    }

    public void DestroyLine(GameObject line)
    {
        healthLine--;
        Destroy(line);
        //StartCoroutine(LineDestruction(line));

        if (healthLine <= 0)
        {
            healthLine = 0;
            onDead = true;
        }
    }


    IEnumerator LineDestruction(GameObject line)
    {
        float lineDestroyTimer = lineTimer;

        Debug.Log("Boom?");
        line.GetComponent<Collider2D>().enabled = false;
        line.GetComponent<Collider2D>().enabled = false;

        while (lineDestroyTimer > 0)
        {
            lineDestroyTimer -= 0.1f;
            yield return null;
        }

        Debug.Log("Boom!");
        DestroyLine(line);
        yield return null;
    }
}
