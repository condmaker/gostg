using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    GameObject               shiki;
    CinemachineVirtualCamera cine;
    public int               teleportPosition;
    public bool              isBoss;

    private EnemiesInArena   eia;
    public bool              warp = false;
    public                   Vector3 posOffset;

    void Start()
    {
        shiki = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
        cine = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        eia = GameObject.FindObjectOfType<EnemiesInArena>();
    }
    void Update()
    {
        if (warp)
        {
            Vector3 oldPos = shiki.transform.position;
            shiki.transform.position = new Vector2(teleportPosition, shiki.transform.position.y);
            posOffset = shiki.transform.position - oldPos;

            cine.OnTargetObjectWarped(shiki.transform, posOffset);

            if (isBoss)
            {
                Debug.Log("List: " + eia.Enemies.Count);
                foreach (GameObject i in eia.Enemies)
                {
                    i.transform.position += posOffset;
                    Debug.Log("Should work lol");
                }
            }

            warp = false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            warp = true;        
    }
}
