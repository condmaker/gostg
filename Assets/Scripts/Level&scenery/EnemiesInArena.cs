using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesInArena : MonoBehaviour
{
    public List<GameObject> Enemies { get; private set; } = new List<GameObject>();
    public AudioClip music;

    private GameObject boss;
    private bool arenaStart = false;
    private GameObject next;

    void Update()
    {
        foreach (GameObject i in Enemies)
        {
            if (i == null)
                Enemies.Remove(i);
        }

        if (boss == null && arenaStart)
        {
            SoundMng.instance.PlayMusic(music);
            arenaStart = false;
            next = GameObject.FindWithTag("nextLevel");

            foreach (Transform child in next.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer != 16) && (other.gameObject.layer != 10))
            return;
        if (other.gameObject.layer == 16)
        {
            boss = other.gameObject;
            arenaStart = true;
        }

        Enemies.Add(other.gameObject);
    }
}
