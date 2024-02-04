using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTricks : MonoBehaviour
{
    public GameManager gameManager;
    private Rigidbody rbVelocity;
    private bool isFlipping = false;
    private bool isSpinning = false;
    private bool tricksEnabled = false;
    private bool justSpun = false;
    private bool justFlipped = false;
    private float flipStartTime;
    private float flipDuration = .875f;
    private float spinStartTime;
    private float spinDuration = .875f;
    private SkiGameInputActions skiGameInputActions;
    private Transform player;
    private Vector3 storedPosition;
    private Quaternion storedRotation;
    
    private void Awake()
    {
        player = GetComponent<Transform>();
        rbVelocity = GetComponent<Rigidbody>();
        skiGameInputActions = new SkiGameInputActions();

        // Subscribe to new input asset SkiGameInputActions Flip and Spin action
        skiGameInputActions.GamePlay.Flip.started += _ => StartFlip();
        skiGameInputActions.GamePlay.Spin.started += _ => StartSpin();
    }



    public void EnableTricks(Vector3 position, Quaternion rotation)
    {
        tricksEnabled = true;
        storedPosition = position;
        storedRotation = rotation;
    }


    public void DisableTricks()
    {
        tricksEnabled = false;
    }

    private void OnEnable()
    {
        skiGameInputActions.Enable();
        //Jumps.OnJumpTriggered += HandleJumpTriggered;
    }

    private void OnDisable()
    {
        skiGameInputActions.Disable();
        //Jumps.OnJumpTriggered -= HandleJumpTriggered;
    }


    private void HandleJumpTriggered(Vector3 position, Quaternion rotation)
    {
        storedPosition = position;
        storedRotation = rotation;
    }

    
    void Update()
    {
        SkiFlip();
        SkiSpin();
    }

    

    public void SkiFlip()
    {

        if (isFlipping)
        {
            if (Time.time - flipStartTime > flipDuration + .075f)
            {
                isFlipping = false;
                rbVelocity.constraints |= RigidbodyConstraints.FreezeRotationX;
                justFlipped = true;
            }
            
        }

        if (!isFlipping && justFlipped)
        {
            justFlipped = false;

            if (Mathf.Abs(player.eulerAngles.x) > 10)
            {
                StartCoroutine(Crash());
            }
        }
    }


    private void StartFlip()
    {
        isFlipping = true;
        flipStartTime = Time.time;
        rbVelocity.constraints &= ~RigidbodyConstraints.FreezeRotationX;
        rbVelocity.AddTorque(-transform.right * 2f, ForceMode.Impulse);
        SoundManager.instance.PlayTrickWhoosh();
    }


public void SkiSpin()
    {
        if (isSpinning)
        {
            if (Time.time - spinStartTime > spinDuration + .075f)
            {
                isSpinning = false;
                rbVelocity.constraints |= RigidbodyConstraints.FreezeRotationY;
                justSpun = true;
            }
            
        }

        if (!isSpinning && justSpun)
        {
            justSpun = false;

            if (Mathf.Abs(player.eulerAngles.y) < 170 || Mathf.Abs(player.eulerAngles.y) > 190)
            {
                StartCoroutine(Crash());
            }
        }
    }


    private void StartSpin()
    {
        isSpinning = true;
        spinStartTime = Time.time;
        rbVelocity.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        rbVelocity.AddTorque(-transform.up * 2f, ForceMode.Impulse);
        SoundManager.instance.PlayTrickWhoosh();
    }


    private IEnumerator Crash()
    {
        SoundManager.instance.PlayCrash();
        PlayerMover playerMover = player.GetComponent<PlayerMover>();
        
        yield return new WaitForSeconds(0.35f);
        gameManager.crashUI.SetActive(true);
        
        // Make text flash on and off rapidly
        for (int i = 0; i < 10; i++)
        {
            gameManager.crashText.enabled = !gameManager.crashText.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        // Lose points for crashing, but don't go below zero
        if (ScoreManager.score > 0)
        {
            ScoreManager.score -= 5;
        }
        else
        {
            ScoreManager.score = 0;
        }

        yield return new WaitForSeconds(0.35f);

        Vector3 resetPositon = new Vector3(
            storedPosition.x,
            storedPosition.y + 1.6f,
            storedPosition.z + 20f);
        
        player.position = resetPositon;
        player.rotation = storedRotation;
        playerMover.ResetPlayerState();
        
        gameManager.crashUI.SetActive(false);

        DisableTricks();
    }

}
