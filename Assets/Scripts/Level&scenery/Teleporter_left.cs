using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter_left : MonoBehaviour
{
    GameObject shiki;
    Camera cam;
    void Start()
    {
        shiki = GameObject.Find("Shiki");
        cam = Camera.main;
    }
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Collided");
        if(other.gameObject.tag == "Player")
        {
            shiki.transform.position = new Vector2(1840, shiki.transform.position.y);
            cam.transform.position = new Vector2(1940, 848);
        }
    }
}
