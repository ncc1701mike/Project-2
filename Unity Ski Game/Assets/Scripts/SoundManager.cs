using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip scoreChimes, treeRockHit, trickWhoosh, rebound, flagHit, skiing, jumpWhoosh,crash;
    public static SoundManager instance;
    public AudioSource skiAudioSource; 
    private AudioSource audioSource; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //skiAudioSource = GetComponent<AudioSource>();
    }

    public void PlayScoreChimes()
    {
        audioSource.volume = 4.5f;
        audioSource.PlayOneShot(scoreChimes);
    }

    public void PlayTreeRockHit()
    {
        audioSource.volume = 15f;
        audioSource.PlayOneShot(treeRockHit);
    }

    public void PlayFlagHit()
    {
        audioSource.PlayOneShot(flagHit);
    }

    public void PlayRebound()
    {
        if (audioSource.volume > 0.25f)
        {
            audioSource.volume = 0.15f;
            audioSource.PlayOneShot(rebound);
        }
    }

    public void PlayTrickWhoosh()
    {
        audioSource.PlayOneShot(trickWhoosh);
    }


    public bool IsSkiingPlaying()
    {
        return skiAudioSource.isPlaying && skiAudioSource.clip == skiing;
    }


    public void PlaySkiing()
    {
        if (!skiAudioSource.isPlaying || skiAudioSource.clip != skiing)
        {
            skiAudioSource.volume = 0.3f;
            skiAudioSource.clip = skiing;
            skiAudioSource.loop = true;
            skiAudioSource.Play();
        }
    }

    public void StopSkiing()
    {
        skiAudioSource.loop = false;
        skiAudioSource.Stop();
    }

    public bool IsJumpWhooshPlaying()
    {
        return audioSource.isPlaying && audioSource.clip == jumpWhoosh;
    }

    public void PlayJumpWhoosh()
    {
        if(!audioSource.isPlaying || audioSource.clip != jumpWhoosh)
        {
            audioSource.volume = 0.05f;
            audioSource.PlayOneShot(jumpWhoosh);
        }
    }

    public void PlayCrash()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(crash);
    }

}
