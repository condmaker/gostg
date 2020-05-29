using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_teleport : MonoBehaviour
{
    GameObject shiki;
    public Animator panel;
    bool elevatorOn;
    void Start()
    {
        shiki = GameObject.Find("Shiki");
    }
    void Update()
    {
        elevatorOn = GameObject.Find("ButtonRight").GetComponent<Button>().elevatorOn;
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
        shiki.transform.localScale = new Vector2(1.5f, 1.5f);
        if (gameObject.name == "DoorElevator1")
        {
            shiki.transform.position = new Vector2(-36, 990);
        }
        else if (gameObject.name == "DoorElevator2")
        {
            shiki.transform.position = new Vector2(509, 990);
        }
        else if (gameObject.name == "DoorElevator3")
        {
            shiki.transform.position = new Vector2(1050, 990);
        }
        else if (gameObject.name == "DoorElevator4")
        {
            shiki.transform.position = new Vector2(1587, 990);
        }
        else if (gameObject.name == "elevatorbutton" && elevatorOn == true)
        {
            shiki.transform.position = new Vector2(2323, 181);
            shiki.transform.localScale = new Vector2(1.2f, 1.2f);
        }
    }
}
