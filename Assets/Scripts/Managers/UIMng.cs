using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMng : MonoBehaviour
{
    public GameObject[] pauseUI;
    public GameObject menu;
    public GameObject selector;
    public HealthPoints shiki_health;
    public SoundMng snd;

    public AudioClip change_option;
    public AudioClip _quit;
    public AudioClip _continue;

    private int distanceToMove;

    // Start is called before the first frame update
    void Start()
    {
        snd = GameObject.FindObjectOfType<SoundMng>();
        DontDestroyOnLoad(gameObject);
        distanceToMove = 69;
    }

    // Update is called once per frame
    void Update()
    {
        shiki_health = GameObject.FindObjectOfType<HealthPoints>();
        Debug.Log(shiki_health.hp);
        if (shiki_health.hp <= 0)
        {
            Time.timeScale = 0;
            Pause();
            menu.SetActive(true);
        }
        if (Input.GetKeyDown("escape"))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Pause();
                menu.SetActive(true);
                foreach (Transform child in snd.gameObject.transform)
                {
                    child.gameObject.GetComponent<AudioSource>().volume -= 0.8f;
                }
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                Unpause();
                menu.SetActive(false);
                foreach (Transform child in snd.gameObject.transform)
                {
                    child.gameObject.GetComponent<AudioSource>().volume += 0.8f;
                }
            }
        }
        if (Input.GetButtonDown("Down") && Time.timeScale == 0)
        { 
            if (selector.transform.position.y < 200)
            {
                SoundMng.instance.PlaySound(change_option, 0.3f);
                selector.transform.position = new Vector3(selector.transform.position.x, selector.transform.position.y + distanceToMove , selector.transform.position.z);
            }
            else
            {
                SoundMng.instance.PlaySound(change_option, 0.3f);
                selector.transform.position = new Vector3(selector.transform.position.x, selector.transform.position.y - distanceToMove,selector.transform.position.z);
            }
        }
        if (Input.GetButtonDown("Up") && Time.timeScale == 0)
        {
            if (selector.transform.position.y > 200)
            {
                SoundMng.instance.PlaySound(change_option, 0.3f);
                selector.transform.position = new Vector3(selector.transform.position.x, selector.transform.position.y - distanceToMove , selector.transform.position.z);
            }
            else
            {
                SoundMng.instance.PlaySound(change_option, 0.3f);
                selector.transform.position = new Vector3(selector.transform.position.x, selector.transform.position.y + distanceToMove, selector.transform.position.z);
            }
        }
        if (Input.GetButtonDown("Enter") && Time.timeScale == 0)
        {
            if (shiki_health.hp <= 0)
            {
                if (selector.transform.position.y > 200)
                {
                    SoundMng.instance.PlaySound(_continue, 0.3f);
                    menu.SetActive(false);
                    Time.timeScale = 1;
                    Unpause();
                    SceneManager.LoadScene("Level1");
                }
                else
                {
                    SoundMng.instance.PlaySound(_quit, 0.3f);
                    SceneManager.LoadScene("TitleScreen");
                }
            }
            else
            {
                if (selector.transform.position.y > 467)
                {
                    SoundMng.instance.PlaySound(_continue, 0.3f);
                    menu.SetActive(false);
                    Time.timeScale = 1;
                    Unpause();
                }
                else
                {
                    SoundMng.instance.PlaySound(_quit, 0.3f);
                    Debug.Log("Quits");
                    Application.Quit();
                }
            }
        }


    }

    void Pause()
    {
        foreach (GameObject g in pauseUI)
        {
            g.SetActive(true);
        }
        return;
    }

    void Unpause()
    {
        foreach (GameObject g in pauseUI)
        {
            g.SetActive(false);
        }
    }

    //if (Input.GetKey("escape"))
    //    {
    //        Application.Quit();
    //    }
}
