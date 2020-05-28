using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string scene;
    public Animator panel;
    public void PlayGame()
    {
        StartCoroutine(Transistion());
    }
    IEnumerator Transistion()
    {
        panel.SetTrigger("fadeout");
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(scene);
    }
}
