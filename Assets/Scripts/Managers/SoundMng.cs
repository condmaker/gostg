using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMng : MonoBehaviour
{
    static public SoundMng instance;

    List<AudioSource> audioSources;
    AudioSource       currentMusic;
    private bool      musicIsPlaying;

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

    public void PlayMusic(AudioClip music, float volume = 1.0f)
    {
        if (musicIsPlaying)
        {
            currentMusic.Stop();
            Destroy(currentMusic);
        }

        AudioSource audioSource = NewSoundObject();
        audioSource.clip = music;
        audioSource.volume = volume;
        audioSource.Play();
        audioSource.loop = true;
        currentMusic = audioSource;

        musicIsPlaying = true;
    }

    public void PlaySound(AudioClip sound, float volume = 0.5f)
    {
        AudioSource audioSource = NewSoundObject();
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.Play();
    }

    AudioSource NewSoundObject()
    {
        foreach (AudioSource audio in audioSources)
        {
            if (!audio.isPlaying)
            {
                return audio;
            }
        }

        GameObject gObject = new GameObject();
        gObject.name = "SoundEffect";
        gObject.transform.parent = transform;
        AudioSource audioSource = gObject.AddComponent<AudioSource>();

        audioSources.Add(audioSource);

        return audioSource;
    }
}
