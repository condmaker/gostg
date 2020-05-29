using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Elevator_teleport : MonoBehaviour
{
    GameObject shiki;
    public Animator panel;

    bool elevatorOn;

    public AudioClip sceneMusic;
    public CinemachineVirtualCamera vcam;
    public GameObject city_left;

    private bool isPressed = false;

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
            city_left.transform.position += new Vector3(0, 200, 0);
        }
        else if (gameObject.name == "DoorElevator4")
        {
            shiki.transform.position = new Vector2(1587, 990);
        }
        else if (gameObject.name == "elevatorbutton" && elevatorOn == true)
        {
            SoundMng.instance.PlayMusic(sceneMusic);

            shiki.transform.position = new Vector2(2353.24f, 181);
            shiki.transform.localScale = new Vector2(0.8f, 0.8f);
            vcam.m_Lens.OrthographicSize = 200;
        }
        else if (gameObject.name == "Parte2")
        {
            shiki.transform.position = new Vector2(3912, -600);
            shiki.transform.localScale = new Vector2(0.8f, 0.8f);
        }

        isPressed = false;
    }
}
