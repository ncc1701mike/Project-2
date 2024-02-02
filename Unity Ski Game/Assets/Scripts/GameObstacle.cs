using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


// GameObstacle is the base class for all obstacles in the game
public class GameObstacle : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerCollide;
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnPlayerCollide?.Invoke(other.gameObject);
            OnObstacleHit(other.gameObject);
        }
    }


    protected virtual void OnObstacleHit(GameObject player)
    {
        // Play impact sound effect
        //audioSource.PlayOneShot(impactSound);
    }

}
