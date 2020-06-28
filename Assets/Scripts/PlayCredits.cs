using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayCredits : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(69));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        text.SetActive(true);
    }
}
