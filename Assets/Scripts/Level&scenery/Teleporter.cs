using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    GameObject shiki;
    CinemachineVirtualCamera cine;
    public int teleportPosition;
    private bool warp = false;

    void Start()
    {
        shiki = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
        cine = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }
    void Update()
    {
        if (warp)
        {
            Vector3 oldPos = shiki.transform.position;

            shiki.transform.position = new Vector2(teleportPosition, shiki.transform.position.y);
            cine.OnTargetObjectWarped(shiki.transform, shiki.transform.position - oldPos);

            warp = false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            warp = true;        
    }
}
