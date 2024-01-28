using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTricks : MonoBehaviour
{
    private Rigidbody rbVelocity;
    private bool isFlipping = false;
    private bool isSpinning = false;
    private bool justSpun = false;
    private bool justFlipped = false;
    private float flipStartTime;
    private float flipDuration = .875f;
    private float spinStartTime;
    private float spinDuration = .875f;
    private SkiGameInputActions skiGameInputActions;
    
    private Transform player;
    public GameManager gameManager;

    
    private void Awake()
    {
        player = GetComponent<Transform>();
        rbVelocity = GetComponent<Rigidbody>();
        skiGameInputActions = new SkiGameInputActions();

        // Subscribe to new input asset SkiGameInputActions Flip and Spin action
        skiGameInputActions.GamePlay.Flip.started += _ => StartFlip();
        skiGameInputActions.GamePlay.Spin.started += _ => StartSpin();
    }

    private void OnEnable()
    {
        skiGameInputActions.Enable();
    }

    private void OnDisable()
    {
        skiGameInputActions.Disable();
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
    }


    private IEnumerator Crash()
    {
        yield return new WaitForSeconds(0.35f);
        gameManager.crashUI.SetActive(true);
        
        // Make text flash on and off rapidly
        for (int i = 0; i < 10; i++)
        {
            gameManager.crashText.enabled = false;
            yield return new WaitForSeconds(0.1f);
            gameManager.crashText.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.35f);
        
        SceneManager.LoadScene(0);

        yield return null;
    }

}
