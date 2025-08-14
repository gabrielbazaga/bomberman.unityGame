using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip backgroundTheme;
    [SerializeField] private AudioClip putABombAudio;
    [SerializeField] private AudioClip bombExplosionAudio;
    [SerializeField] private AudioClip getPowerUpAudio;

    void Start()
    {
        instance = this;
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public AudioClip PutABombAudio { get => putABombAudio; set => putABombAudio = value; }
    public AudioClip BombExplosionAudio { get => bombExplosionAudio; set => bombExplosionAudio = value; }
    public AudioClip GetPowerUpAudio { get => getPowerUpAudio; set => getPowerUpAudio = value; }
    public AudioClip BackgroundTheme { get => BackgroundTheme; set => BackgroundTheme = value; }
    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
}
