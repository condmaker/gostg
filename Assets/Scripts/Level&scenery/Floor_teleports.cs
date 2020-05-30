using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_teleports : MonoBehaviour
{
    public Transform shikiTargetPosition;
    public Transform cityTargetPosition;

    GameObject shiki;
    public Animator panel;
    public GameObject city_left;

    bool keyPressed;

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
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.GetComponentInParent<PlayerMovement>() != null) && (keyPressed))
        //if (other.gameObject.tag == "Player" && keyPressed)
        {
            StartCoroutine(Teleport());
        }
    }
    IEnumerator Teleport()
    {
        panel.SetTrigger("fadeout");
        yield return new WaitForSeconds(1.3f);
        shiki.transform.localScale = new Vector2(0.8f, 0.8f);
        shikiTargetPosition.position += new Vector3(0, 0, -shikiTargetPosition.position.z);
        shiki.transform.position = shikiTargetPosition.position;
        if (cityTargetPosition)
        {
            city_left.transform.position = cityTargetPosition.position;
        }
    }
}

