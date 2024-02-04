using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : GameObstacle
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        SoundManager.instance.PlayTreeRockHit();

        // Deduct points for collision with rock
        // Condition to make sure player score does not go below 0
        if (ScoreManager.score > 0)
        {
            ScoreManager.score -= 10;
        }
        else
        {
            ScoreManager.score = 0;
        }
    }
}
