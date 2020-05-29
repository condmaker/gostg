﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool elevatorOn;

    public GameObject puppet;
    public Transform button;
    public GameObject light;
    public GameObject light2;

    private bool isPressed = false;

    void Start()
    {
    }
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.S))
        {
            if (gameObject.name == "wrongbutton1")
            {
                Instantiate(puppet, new Vector3(860, 3), transform.rotation);
                light2.SetActive(true);
            }
            else if (gameObject.name == "wrongbutton2")
            {
                Instantiate(puppet, new Vector3(860, -467), transform.rotation);
                light2.SetActive(true);
            }
            else if (gameObject.name == "wrongbutton3")
            {
                Instantiate(puppet, new Vector3(860, -980), transform.rotation);
                light2.SetActive(true);
            }
            else if (gameObject.name == "ButtonRight")
            {
                light.SetActive(true);
                light2.SetActive(true);
                elevatorOn = true;
            }

        }
    }
}
