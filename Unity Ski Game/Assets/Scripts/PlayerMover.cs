﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    private Transform player;
    private Animator playerAnimator;
    private PlayerBoost playerBoost;
    private SkiGameInputActions skiGameInputActions;
    private Rigidbody rbVelocity;
    public float maxSpeed = 25f;
    public float currentSpeed;


    private void Awake()
    {
        skiGameInputActions = new SkiGameInputActions();
        rbVelocity = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        playerBoost = GetComponent<PlayerBoost>();

        // Subscribe to new input asset SkiGameInputActions Boost action
        skiGameInputActions.GamePlay.LeftTurn.performed += ctx => TurnLeft(ctx.ReadValue<float>());
        skiGameInputActions.GamePlay.RightTurn.performed += ctx => TurnRight(ctx.ReadValue<float>());
    }


    private void OnEnable()
    {
        skiGameInputActions.Enable();
    }


    private void OnDisable()
    {
        skiGameInputActions.Disable();
    }

    
    private void FixedUpdate()
    {
        VelocityPerRotation();
        SkierAnimation();
    }


    void Update()
    {
        TurnPlayer();
        PlayerSpeed();
    }


    private void TurnPlayer()
    {
       if (Physics.Raycast(player.position, Vector3.down, 0.1f))
        {
            float yRotation = NormalizeAngle(player.eulerAngles.y);
            float turnLeftValue = skiGameInputActions.GamePlay.LeftTurn.ReadValue<float>();
            float turnRightValue = skiGameInputActions.GamePlay.RightTurn.ReadValue<float>();

            if (turnLeftValue > 0.5f && yRotation < 270)
            {
                player.Rotate(0, -30 * Time.deltaTime, 0);
            }
            else if (turnRightValue > 0.5f && yRotation > 90)
            {
                player.Rotate(0, 30 * Time.deltaTime, 0);
            }
        }
    }


    private void TurnLeft(float value)
    {
        // You can add additional logic if needed
    }


    private void TurnRight(float value)
    {
        // You can add additional logic if needed
    }


    public void PlayerSpeed()
    {
        currentSpeed = rbVelocity.velocity.magnitude;
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

        if (playerBoost.IsBoostActive())
        {
            downHillVelocity += playerBoost.GetBoostVelocity();
        }

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
    

    private void SkierAnimation()
    {
        float speedPercent = rbVelocity.velocity.magnitude / maxSpeed;
        float transitionThreshold = 0.25f;
        float buffer = 0.20f;  // Adjust this buffer as needed

        // Trigger the transition slightly before reaching the threshold
        if (speedPercent < (transitionThreshold - buffer))
        {
            playerAnimator.SetFloat("SpeedPercent", 0f); // Force transition to SkiSlow
        }
        else
        {
            playerAnimator.SetFloat("SpeedPercent", speedPercent);
        }
    }  
}
