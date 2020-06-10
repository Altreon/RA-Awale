using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    static SoundBox instance;

    [SerializeField]
    AudioSource ambiantSource = null;
    [SerializeField]
    AudioSource soundSource = null;

    [SerializeField]
    AudioClip ambiant = null;
    [SerializeField]
    AudioClip seedTake = null;
    [SerializeField]
    AudioClip seedPut = null;
    [SerializeField]
    AudioClip seedEarn = null;
    [SerializeField]
    AudioClip playerSwitch = null;

    [SerializeField]
    float loodSondVolume = 0.5f;

    public static SoundBox Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start () {
        ambiantSource.clip = ambiant;
        ambiantSource.volume = loodSondVolume;
        PlayAmbiant();
    }

    public void PlayAmbiant () {
        ambiantSource.Play();
    }

    public void MuteAmbiant () {
        ambiantSource.Stop();
    }

    public void PlaySeedTake () {
        soundSource.PlayOneShot(seedTake);
    }

    public void PlaySeedPut () {
        soundSource.PlayOneShot(seedPut, loodSondVolume);
    }

    public void PlaySeedEarn () {
        soundSource.PlayOneShot(seedEarn, loodSondVolume);
    }

    public void PlayPlayerSwitch () {
        soundSource.PlayOneShot(playerSwitch, loodSondVolume);
    }
}
