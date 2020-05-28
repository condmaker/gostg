using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    EnemyHealth health;

    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("SliderEnemy").GetComponent<Slider>();
        health = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health.enemyHealth;

        //if (collision.)
    }
}
