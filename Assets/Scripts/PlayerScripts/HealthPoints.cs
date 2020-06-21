using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthPoints : MonoBehaviour
{
    public float       hp = 100;
    public float       invulTime = 2f;
    public float       attackCooldownTimer;
    public bool        isInvul = false;
    public bool        isInmove = false;
    public Slider      slider;
    public Slider      Stamina;

    public delegate void OnDead();
    public event OnDead onDead;

    public delegate void OnHit(Vector2 direction);
    public event OnHit onHit;

    public Rigidbody2D playerBody;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        slider = GameObject.Find("Health").GetComponent<Slider>();
        Stamina = GameObject.Find("Stamina").GetComponent<Slider>();
    }

    void Update()
    {
        attackCooldownTimer = GameObject.FindObjectOfType<PlayerAttack>().attackCooldownTimer;
        Stamina.value = attackCooldownTimer;
        if (isInvul)
        {
            invulTime -= Time.deltaTime;
            if (invulTime <= 1.5)
            {
                isInmove = false;
            }
            if (invulTime <= 0)
            {
                invulTime = 2f;
                isInvul = false;
            }
        }
    }

    // TODO - Make the entity fickle when invunerable
    public void DealDamage(int damage, Vector2 direction)
    {
        if (hp < 0) return;

        if (!isInvul)
        {
            hp -= damage;
            isInvul = true;
            isInmove = true;

            onHit(direction);
            slider.value  = hp;
        }

        if (hp <= 0)
        {
            hp = 0;
            onDead();
            SceneManager.LoadScene("Level1");
        }

    }
}
