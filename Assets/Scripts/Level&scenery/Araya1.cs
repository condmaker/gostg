using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Araya1 : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Testy1");

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Testy2");
            cam.LookAt = transform;
        }
    }
}
