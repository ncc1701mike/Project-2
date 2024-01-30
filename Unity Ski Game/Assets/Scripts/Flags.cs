using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : GameObstacle
{
    public bool isCorrectSide;
    protected override void OnTriggerEnter(Collider other)
    {
       base.OnTriggerEnter(other);
        if (isCorrectSide)
        {
            // Add points
            ScoreManager.score += 5;
        }
        else
        {
            // Deduct points
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
}

