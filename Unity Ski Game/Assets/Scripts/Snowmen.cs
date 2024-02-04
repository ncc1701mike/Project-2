using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowmen : GameObstacle
{
    protected override void OnObstacleHit(GameObject player)
    {
        // Score points
        ScoreManager.score += 10;

        // Score Chime
        SoundManager.instance.PlayScoreChimes();
        
        // Call the base class method
        base.OnObstacleHit(player);
        
        // Destroy the snowman
        Destroy(gameObject);
    }
}
