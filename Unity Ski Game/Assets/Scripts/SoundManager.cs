using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip scoreChimes, treeRockHit, trickWhoosh, rebound, flagHit, skiing, jumpWhoosh,crash;
    private AudioSource audioSource;

    // Start is called before the first frame update
    
    public static SoundManager instance;

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
    }

    public void PlayScoreChimes()
    {
        audioSource.volume = 2f;
        audioSource.PlayOneShot(scoreChimes);
    }

    public void PlayTreeRockHit()
    {
        audioSource.volume = 2.5f;
        audioSource.PlayOneShot(treeRockHit);
    }

    public void PlayFlagHit()
    {
        audioSource.PlayOneShot(flagHit);
    }

    public void PlayRebound()
    {
        audioSource.volume = 0.1f;
        audioSource.PlayOneShot(rebound);
    }

    public void PlayTrickWhoosh()
    {
        audioSource.PlayOneShot(trickWhoosh);
    }

    public void PlaySkiing(float delayInSeconds = 0.75f)
    {
        // loop the audioclip
        if(!audioSource.isPlaying || audioSource.clip != skiing)
        {
            audioSource.volume = 0.75f;
            audioSource.clip = skiing;
            audioSource.loop = true;
            audioSource.PlayDelayed(delayInSeconds);
        }
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
