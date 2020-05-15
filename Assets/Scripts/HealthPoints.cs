using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoints : MonoBehaviour
{
    public float       hp = 100;
    public float       invulTime = 0.5f;
    public bool        isInvul = false;
    public Slider slider;

    public delegate void OnDead();
    public event OnDead onDead;

    public delegate void OnHit();
    public event OnHit onHit;

    public Rigidbody2D playerBody;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        slider = GameObject.Find("SliderPlayer").GetComponent<Slider>();
    }

    void Update()
    {
        if (isInvul)
        {
            invulTime -= Time.deltaTime;
            if (invulTime <= 0)
            {
                invulTime = 0.5f;
                isInvul = false;
            }
        }
    }

    // TODO - Make the entity fickle when invunerable
    public void DealDamage(int damage)
    {
        if (hp < 0) return;

        if (!isInvul)
        {
            hp -= damage;
            isInvul = true;

            onHit();
            slider.value  = hp;
        }

        if (hp <= 0)
        {
            onDead();
        }

    }
}
