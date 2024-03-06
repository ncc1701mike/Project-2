using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Flags : MonoBehaviour
{

    private float raceStartTime;
    private float timeSinceRaceStart;
    private bool isActive;

    private void OnEnable()
    {
        FlagEvents.OnFlagReached += HandleFlagReached;
    }

    private void OnDisable()
    {
        FlagEvents.OnFlagReached -= HandleFlagReached;
    }

    private void HandleFlagReached(FlagEventArgs e)
    {
        if (e.FlagType == "StartingFlag")
        {
            RaceTimer();
        }

        else if (e.FlagType == "BlueFlag" || e.FlagType == "PinkFlag")
        {
            MissedFlag();
        }
    }

    //Trigger the FlagReached event when the player collides with the flag
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            string flagType = gameObject.tag;
            FlagEvents.FlagReached(flagType);
        }

    }

    private void RaceTimer()
    {
        isActive = true;
        raceStartTime = Time.time; // get the time when the player reaches the starting flag
        GameManager.instance.ShowRaceTimer();
    }

    private void MissedFlag()
    {
        if (isActive)
        {
            raceStartTime -= 1f;
        }
    }


    private void Update()
    {
        if (isActive)
        {
            timeSinceRaceStart = Time.time - raceStartTime; // calculate the time difference between the current time and the start time 
        }
    }

    public float GetTimeSinceRaceStart()
    {
        Debug.Log("Time since race start: " + timeSinceRaceStart + " seconds");
        return timeSinceRaceStart;
    }

}
