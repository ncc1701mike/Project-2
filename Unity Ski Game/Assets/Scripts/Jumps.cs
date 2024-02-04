using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Jumps : MonoBehaviour
{
//public static event Action<Vector3, Quaternion> OnJumpTriggered;

 private void OnTriggerEnter(Collider other)
    {
       /* if (other.CompareTag("Player"))
        {
            Vector3 storedPosition = other.transform.position;
            Quaternion storedRotation = other.transform.rotation;

            OnJumpTriggered?.Invoke(transform.position, transform.rotation);
        }*/

         if (other.CompareTag("Player"))
        {
            PlayerTricks playerTricks = other.GetComponent<PlayerTricks>();
            if (playerTricks != null)
            {
                playerTricks.EnableTricks(other.transform.position, other.transform.rotation);
            }
        }

    }   
}
