using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_teleports : MonoBehaviour
{
    GameObject shiki;
    public Animator panel;
    void Start()
    {
        shiki = GameObject.Find("Shiki");
    }
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown("s"))
        {
            Debug.Log("The~Door");
            StartCoroutine(Teleport());
        }
    }
    IEnumerator Teleport()
    {
        panel.SetTrigger("fadeout");
        yield return new WaitForSeconds(1.3f);
        shiki.transform.localScale = new Vector2(0.8f, 0.8f);
        shiki.transform.position = new Vector2(1000, 0);
    }
}

