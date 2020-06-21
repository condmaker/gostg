using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Elevator_teleport : MonoBehaviour
{
    public Transform shikiTargetPosition;

    GameObject shiki;
    public Animator panel;

    bool elevatorOn;

    public AudioClip sceneMusic;
    public CinemachineVirtualCamera vcam;
    public GameObject city_left;

    private bool keyPressed = false;
    void Start()
    {
        shiki = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
            keyPressed = true;
        else
            keyPressed = false;

        elevatorOn = GameObject.Find("ButtonRight").GetComponent<Button>().elevatorOn;
        Debug.Log(elevatorOn);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && keyPressed)
        {
            StartCoroutine(Teleport());
        }
    }
    IEnumerator Teleport()
    {
        keyPressed = true;

        panel.SetTrigger("fadeout");
        yield return new WaitForSeconds(1.3f);

        if (gameObject.name == "Parte2")
        {
            shiki.transform.position = shikiTargetPosition.position; ;
            shiki.transform.localScale = new Vector2(0.8f, 0.8f);
        }
        else if (gameObject.name == "Parte3")
        {
            shiki.transform.position = shikiTargetPosition.position;
            shiki.transform.localScale = new Vector2(0.8f, 0.8f);
        }
        else if (gameObject.name == "elevatorbutton" && elevatorOn == true)
        {
            SoundMng.instance.PlayMusic(sceneMusic);

            shiki.transform.position = shikiTargetPosition.position;
            shiki.transform.localScale = new Vector2(0.8f, 0.8f);
            vcam.m_Lens.OrthographicSize = 200;
        }
        else if (gameObject.name != "elevatorbutton")
        {
            shiki.transform.position = new Vector3(shikiTargetPosition.position.x, shikiTargetPosition.position.y + 100, 0);
            shiki.transform.localScale = new Vector2(1.5f, 1.5f);
        }


        keyPressed = false;
    }
}
