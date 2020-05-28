using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIdentifier : MonoBehaviour
{
    public AudioClip sceneMusic;

    void Start()
    {
        SoundMng.instance.PlayMusic(sceneMusic);
    }

}
