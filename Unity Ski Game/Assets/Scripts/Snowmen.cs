using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowmen : GameObstacle
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        // Score points
        ScoreManager.score += 10;
        
        // Destroys the snowman
        Destroy(gameObject); 
        
        // Additional logic for explosion effect
         //Instantiate(explosion, transform.position, transform.rotation);
    }

    
}
