using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesInArena : MonoBehaviour
{
    public List<GameObject> Enemies { get; private set; } = new List<GameObject>();

    void Update()
    {
        foreach (GameObject i in Enemies)
        {
            if (i == null)
                Enemies.Remove(i);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer != 16) && (other.gameObject.layer != 10))
            return;

        Debug.Log("Enter!");

        Enemies.Add(other.gameObject);
    }
}
