using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMng : MonoBehaviour
{
    public GameObject[] pauseUI;
    public GameObject menu;
    public GameObject selector;

    public int[] selector_positions;
    private int distanceToMove;
    private int posIndex;
    private float arrowInput;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        distanceToMove = 100;
        selector_positions[0] = 146;
        posIndex = 0;
        for (int i = 1; i < 4; i++)
            {
            selector_positions[i] = selector_positions[i - 1] - distanceToMove;
            }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("in update");
        if (Input.GetKeyDown("escape"))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Pause();
                menu.SetActive(true);
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                Unpause();
                menu.SetActive(false);
            }
        }
        if (Input.GetButton("Down") && Time.timeScale == 0)
        {
            Debug.Log("clicked");
            if (posIndex >= 4)
            {
                posIndex = 0;
            }
            else
            {
                selector.transform.position = new Vector3(selector.transform.position.x, selector_positions[posIndex] - distanceToMove ,selector.transform.position.z);
            }
        }
        if (Input.GetButton("Up") && Time.timeScale == 0)
        {
            Debug.Log(selector_positions[0]);
            if (posIndex >= 4)
            {
                posIndex = 0;
            }
            else
            {
                selector.transform.position = new Vector3(selector.transform.position.x, selector_positions[posIndex] - distanceToMove, selector.transform.position.z);
            }
        }


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
