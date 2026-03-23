using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : SingleTon<AudioManager>
{
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM(string path)
    {
        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
    }

    public void PlayClip(string path, float volume)
    {
        AudioClip ac = Resources.Load<AudioClip>(path);
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position, volume);
    }
}