using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_teleports : MonoBehaviour
{
    GameObject shiki;
    public Animator panel;
    void Start()
    {
        shiki = GameObject.Find("Shiki");
    }
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown("s"))
        {
            StartCoroutine(Teleport());
        }
    }
    IEnumerator Teleport()
    {
        panel.SetTrigger("fadeout");
        yield return new WaitForSeconds(1.3f);
        shiki.transform.localScale = new Vector2(0.8f, 0.8f);
        if (gameObject.name == "Door401405")
        {
            shiki.transform.position = new Vector2(-2, 0);
        }
        else if (gameObject.name == "Door406410")
        {
            shiki.transform.position = new Vector2(-2, -473);
        }
        else if (gameObject.name == "Door411415")
        {
            shiki.transform.position = new Vector2(-2, -713);
        }
        else if (gameObject.name == "Door416420")
        {
            shiki.transform.position = new Vector2(-2, -1000);
        }

    }
}

