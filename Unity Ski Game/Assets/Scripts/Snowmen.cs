using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowmen : GameObstacle
{
    public GameObject explosionParticles;
    protected override void OnObstacleHit(GameObject player)
    {
        // Score points
        ScoreManager.score += 10;

        // Score Chime
        SoundManager.instance.PlayScoreChimes();
        
        // Call the base class method
        base.OnObstacleHit(player);
        
        // Destroy the snowman
        StartCoroutine(Explode());
    }


    private IEnumerator Explode()
    {
        
        Instantiate(explosionParticles, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.25f);

        Destroy(gameObject);
    }

}
