using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanMover : MonoBehaviour
{
    private class SnowmanData
    {
        public GameObject snowmanObject;
        public float speed;
        public float distance;
        public float originalXPosition;
        public float targetXPosition;

        public SnowmanData(GameObject snowmanObject, float speed, float distance)
        {
            this.snowmanObject = snowmanObject;
            this.speed = speed;
            this.distance = distance;
            originalXPosition = snowmanObject.transform.position.x;
            targetXPosition = originalXPosition + distance;
        }
    }

   private List<SnowmanData> snowmanDataList = new List<SnowmanData>();
    
    
    private void Awake()
    {
        // Add all Snowman GameObjects to List
        foreach (GameObject snowman in GameObject.FindGameObjectsWithTag("Snowman"))
        {
            float randomSpeed = Random.Range(1f, 2f);
            float randomDistance = Random.Range(20f, 50f);
            snowmanDataList.Add(new SnowmanData(snowman, randomSpeed, randomDistance));
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        List<SnowmanData> snowmenToRemove = new List<SnowmanData>();
        // Create Lerp to move snowmen from side to side
        foreach (SnowmanData snowmanData in snowmanDataList)
        {
            if (snowmanData.snowmanObject == null)
            {
                snowmenToRemove.Add(snowmanData);
            }
            else
            {
                float newXPosition = Mathf.Lerp(snowmanData.originalXPosition, snowmanData.targetXPosition, Mathf.PingPong(Time.time * snowmanData.speed, 1));
                snowmanData.snowmanObject.transform.position = new Vector3(newXPosition, snowmanData.snowmanObject.transform.position.y, snowmanData.snowmanObject.transform.position.z);
            }
        }

        // Remove snowmen from List when they are destroyed
        foreach (SnowmanData snowmanData in snowmenToRemove)
        {
            snowmanDataList.Remove(snowmanData);
        }
        
    }          
}
