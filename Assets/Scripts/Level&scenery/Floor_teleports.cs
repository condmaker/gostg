﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_teleports : MonoBehaviour
{
    GameObject        shiki;
    public Animator   panel;
    public GameObject city_left;

    private bool      isPressed = false;

    void Start()
    {
        shiki = GameObject.Find("Shiki");
    }
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.S) && !isPressed)
        {
            StartCoroutine(Teleport());
        }
    }
    IEnumerator Teleport()
    {
        isPressed = true;

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
            city_left.transform.position -= new Vector3(0, 200, 0);
            shiki.transform.position = new Vector2(-2, -713);
        }
        else if (gameObject.name == "Door416420")
        {
            shiki.transform.position = new Vector2(-2, -1000);
        }

        isPressed = false;

    }
}

