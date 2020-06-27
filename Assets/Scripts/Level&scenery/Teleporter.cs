using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    GameObject               shiki;
    CinemachineVirtualCamera cine;
    public int               teleportPosition;
    public int               teleportPositionY;
    public bool              isY;
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

            if (!isY)
                shiki.transform.position = new Vector2(teleportPosition, shiki.transform.position.y);
            else
                shiki.transform.position = new Vector2(shiki.transform.position.x, teleportPositionY);

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
