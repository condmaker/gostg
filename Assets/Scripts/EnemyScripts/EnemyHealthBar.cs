using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    HealthPoints health;

    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("SliderEnemy").GetComponent<Slider>();
        health = GetComponent<HealthPoints>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health.hp;

        //if (collision.)
    }
}
