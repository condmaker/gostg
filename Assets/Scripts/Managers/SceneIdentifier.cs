using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneIdentifier : MonoBehaviour
{
    public AudioClip sceneMusic;

    void Start()
    {
        SoundMng.instance.PlayMusic(sceneMusic);

        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            UIMng pause = GameObject.FindObjectOfType<UIMng>();

            if (pause != null)
                Destroy(pause);
        }
    }

}
