using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBehaviour : MonoBehaviour
{
    private Animator anim;
    private float timer = 1.09f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
            Destroy(transform.gameObject);
    }
}
