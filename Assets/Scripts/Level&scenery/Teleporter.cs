using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    GameObject               shiki;
    CinemachineVirtualCamera cine;
    public int               teleportPosition;
    public GameObject        boss;

    public bool              warp = false;
    public                   Vector3 posOffset;

    void Start()
    {
        shiki = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
        cine = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }
    void Update()
    {
        boss = GameObject.FindObjectOfType<HA_Movement>().gameObject;

        if (warp)
        {
            Vector3 oldPos = shiki.transform.position;
            shiki.transform.position = new Vector2(teleportPosition, shiki.transform.position.y);
            posOffset = shiki.transform.position - oldPos;

            cine.OnTargetObjectWarped(shiki.transform, posOffset);
            
            if (boss != null)
                boss.transform.position += posOffset;

            warp = false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            warp = true;        
    }
}
