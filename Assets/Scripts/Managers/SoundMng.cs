using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMng : MonoBehaviour
{
    static public SoundMng instance;

    List<AudioSource> audioSources;
    AudioSource       currentMusic;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        audioSources = new List<AudioSource>();
    }

    public void PlayMusic(AudioClip music, bool loop = true, float volume = 1.0f)
    {
        if (currentMusic != null && currentMusic.isPlaying)
        {
            Debug.Log("Test");
            currentMusic.Stop();
        }

        AudioSource audioSource = NewSoundObject();
        audioSource.clip = music;
        audioSource.volume = volume;
        audioSource.Play();

        if (loop == true)
            audioSource.loop = true;
        else
            audioSource.loop = false;

        currentMusic = audioSource;

    }

    public void PlaySound(AudioClip sound, float volume = 0.5f)
    {
        AudioSource audioSource = NewSoundObject();
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.Play();
    }

    private AudioSource NewSoundObject()
    {

        foreach (AudioSource audio in audioSources)
        {
            if (!audio.isPlaying)
                return audio;
        }

        GameObject gObject = new GameObject();
        gObject.name = "SoundEffect";
        gObject.transform.parent = transform;
        AudioSource audioSource = gObject.AddComponent<AudioSource>();

        audioSources.Add(audioSource);

        return audioSource;
    }
}
