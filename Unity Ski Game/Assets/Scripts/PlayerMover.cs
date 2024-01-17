using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public Transform groundCheck;
    public float maxSpeed = 10f;
    private Rigidbody rbVelocity;
    private Transform player;

   

    private void Awake()
    {
        rbVelocity = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
    }

     void Start()
    {
        
    }
    private void FixedUpdate()
    {
        VelocityPerRotation();
    }

    void Update()
    {
        TurnPlayer();
    }
 
    private void TurnPlayer()
    {
       if (Physics.Raycast(player.position, Vector3.down, 0.1f))
        {
            float yRotation = NormalizeAngle(player.eulerAngles.y);

            if (Input.GetKey("left") && yRotation < 270)
            {
                player.Rotate(0, 1, 0);
            }
            else if (Input.GetKey("right") && yRotation > 90)
            {
                player.Rotate(0, -1, 0);
            }
        }
        else
        {
            float yRotation = 180f;
        }
    }


    private void VelocityPerRotation()  // Rigidbody velocity based on angle difference between player and downhill direction
    {
        float yRotate = NormalizeAngle(rbVelocity.transform.eulerAngles.y);
        float downHillDirection = 180f;
        float angleDifference = Mathf.Abs(yRotate - downHillDirection);
        float velocityFactor;
        
        if (angleDifference == 90) // Explicitly set to zero at 90 degrees
        {
            velocityFactor = 0f;
        }
        else
        {
            velocityFactor = Mathf.Cos(angleDifference * Mathf.Deg2Rad) * 0.5f + 0.5f;
            velocityFactor = Mathf.Clamp(velocityFactor, 0f, 1f);
        }

        Vector3 downHillVelocity = transform.forward * maxSpeed * velocityFactor;
        downHillVelocity.y = rbVelocity.velocity.y;
        rbVelocity.velocity = downHillVelocity; 
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
}
