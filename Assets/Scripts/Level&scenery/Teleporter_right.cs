using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Teleporter_right : MonoBehaviour
{
    GameObject shiki;
    void Start()
    {
        shiki = GameObject.Find("Shiki");
    }
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            shiki.transform.position = new Vector2(-300, 848);
        }
    }
}