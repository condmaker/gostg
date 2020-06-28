using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayNextscene : MonoBehaviour
{
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(3.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("Cutscene 2");
    }
}
