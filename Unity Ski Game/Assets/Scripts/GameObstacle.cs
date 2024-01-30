using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObstacle : MonoBehaviour
{
    // GameObstacle is the base class for all obstacles in the game
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Common collision handling logic (if any)
    }

}
