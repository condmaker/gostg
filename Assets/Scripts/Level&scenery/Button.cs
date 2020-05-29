using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    bool elevatorOn;
    public GameObject puppet;
    public Transform button;
    void Start()
    {
    }
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown("s"))
        {
            if (gameObject.name == "wrongbutton1")
            {
                Instantiate(puppet, new Vector3(860, 3), transform.rotation);
            }
            else if (gameObject.name == "wrongbutton2")
            {
                Instantiate(puppet, new Vector3(860, -467), transform.rotation);
            }
            else if (gameObject.name == "wrongbutton3")
            {
                Instantiate(puppet, new Vector3(860, -980), transform.rotation);
            }
            else if (gameObject.name == "RightButton")
            {
                elevatorOn = true;
            }

        }
    }
}
