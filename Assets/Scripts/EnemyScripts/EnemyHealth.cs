using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int enemyHealth = 50;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("SliderEnemy").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealth;
    }
}
