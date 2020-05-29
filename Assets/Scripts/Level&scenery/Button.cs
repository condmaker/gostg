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
            if (gameObject.name == "WrongButton1")
            {
                Instantiate(puppet, button.position - new Vector2(300, transform.position.y), transform.rotation);
            }
            else if (gameObject.name == "WrongButton2")
            {
                Instantiate(puppet, button.position - new Vector2(300, transform.position.y));
            }
            else if (gameObject.name == "wrongButton3")
            {
                Instantiate(puppet, button.position - new Vector2(300, transform.position.y));
            }
            else if (gameObject.name == "RightButton")
            {
                
            }

        }
    }
}
