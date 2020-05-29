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

    private bool isPressed = false;
    bool keyPressed;

    void Start()
    {
        shiki = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
    }
    void Update()
    {
        elevatorOn = GameObject.Find("ButtonRight").GetComponent<Button>().elevatorOn;
        keyPressed = Input.GetKeyDown(KeyCode.S);

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && (keyPressed) && !isPressed)
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
        shiki.transform.position = shikiTargetPosition.position;
        if (gameObject.name == "elevatorbutton" && elevatorOn == true)
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
