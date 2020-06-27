﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMng : MonoBehaviour
{
    public GameObject[] pauseUI;
    public GameObject Menu;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Pause();
                Menu.SetActive(true);
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                Unpause();
                Menu.SetActive(false);
            }
        }
        //if (Input.GetKeyDown("arrow down") && Time.timeScale == 0)


    }

    void Pause()
    {
        foreach (GameObject g in pauseUI)
        {
            g.SetActive(true);
        }
        return;
    }

    void Unpause()
    {
        foreach (GameObject g in pauseUI)
        {
            g.SetActive(false);
        }
    }

    //if (Input.GetKey("escape"))
    //    {
    //        Application.Quit();
    //    }
}
