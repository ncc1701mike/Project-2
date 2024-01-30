using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoost : MonoBehaviour
{
    private Rigidbody rbVelocity;
    private SkiGameInputActions skiGameInputActions;
    private bool isBoosting = false;
    private float boostAmount = 10f;
    public GameObject boostButton;
    private Color originalColor;
    
    private void Awake()
    {
        rbVelocity = GetComponent<Rigidbody>();
        skiGameInputActions = new SkiGameInputActions();
        InitializeBoostButtonTextColor();

        // Subscribe to new input asset SkiGameInputActions Boost action
        skiGameInputActions.GamePlay.Boost.started += _ => StartBoost();
        skiGameInputActions.GamePlay.Boost.canceled += _ => StopBoost();
    }

    private void OnEnable()
    {
        skiGameInputActions.Enable();
    }

    private void OnDisable()
    {
        skiGameInputActions.Disable();
    }

     public void StartBoost()
    {
        isBoosting = true;
        StopAllCoroutines();
        StartCoroutine(Boost());
        SetBoostButtonTextColor(Color.white);
    }

    public void StopBoost()
    {
        isBoosting = false;
        SetBoostButtonTextColor(originalColor);
    }

    public bool IsBoostActive()
    {
        return isBoosting;
    }

    public Vector3 GetBoostVelocity()
    {
        return isBoosting ? transform.forward * boostAmount : Vector3.zero;
    }

    private IEnumerator Boost()
    {
        float boostDuration = 1f;
        float time = 0f;

        Vector3 initialVelocity = rbVelocity.velocity;
        Vector3 targetVelocity = initialVelocity + transform.forward * boostAmount;

        while (time < boostDuration && rbVelocity.velocity.magnitude < targetVelocity.magnitude)
        {
                rbVelocity.velocity = Vector3.Lerp(initialVelocity, targetVelocity, time / boostDuration);
                time += Time.deltaTime;
                yield return null;
        }

        StartCoroutine(SlowDown(initialVelocity, boostDuration));
    }

    private IEnumerator SlowDown(Vector3 initialVelocity, float duration)
    {
        float time = 0f;
        Vector3 currentVelocity = rbVelocity.velocity;

        while (time < duration)
        {
            rbVelocity.velocity = Vector3.Lerp(currentVelocity, initialVelocity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rbVelocity.velocity = initialVelocity;
    }

    private void InitializeBoostButtonTextColor()
    {
        Text buttonText = boostButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            originalColor = buttonText.color;
        }
        else
        {
            Debug.LogError("Text component is not found in the children of BoostButton in PlayerMover");
        }
    }


    private void SetBoostButtonTextColor(Color color)
    {
        if (boostButton != null)
        {
            Text buttonText = boostButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.color = color;
            }
        }
    }
}
