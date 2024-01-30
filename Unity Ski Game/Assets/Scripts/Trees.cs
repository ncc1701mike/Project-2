using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : GameObstacle
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        // Deduct points for collision with tree
        
        // Condition to make sure player score does not go below 0
        if (ScoreManager.score > 0)
        {
            ScoreManager.score -= 5;
        }
        else
        {
            ScoreManager.score = 0;
        }
    }
    
}
