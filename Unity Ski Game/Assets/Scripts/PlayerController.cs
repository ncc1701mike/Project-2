using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Contains tunable parameters to tweak the player's movement.
    /// </summary>
    [System.Serializable]
    public struct Stats
    {
        [Header("Movement Settings")]
        [Tooltip("The player's current speed.")]
        public float speed;

        [Tooltip("The fastest speed the player can go.")]
        public float speedMaximum;

        [Tooltip("The slowest speed the player can drop to.")]
        public float speedMinimum;

        [Tooltip("How fast the player turns left and right.")]
        public float turnSpeed;

        //TODO (Will): Remove for Course 2 Session 2. Added in video.
        [Tooltip("How much speed the player picks up as they're turning towrds the center.")]
        public float turnAcceleration;

        //TODO (Will): Remove for Course 2 Session 2.
        [Tooltip("How much speed the player drops as they're turning towards the sides.")]
        public float turnDeceleration;

    }

    public Stats playerStats;

    [Tooltip("Keyboard controls for steering left and right.")]
    public KeyCode left, right;

    [Tooltip("Whether the player is moving down hill or not.")]
    public bool isMoving;

    [Tooltip("Child GameObject to check if we are on the ground")]
    public Transform groundCheck;

    [Tooltip("Layermask to hold layers to check for being grounded")]
    public LayerMask groundLayers;

    public bool canMove { get; set; } = true;
    private PlayerBoost playerBoost;
    private bool isSkiAudioPlaying = false;
    private Rigidbody rb;
    private Transform player;
    private Animator animator;
    private Quaternion initialRotation;
    private SkiGameInputActions skiGameInputActions;

    //TODO (Will): Remove for Course 2 Session 2.
    //private PlayerDamage playerDamage;


    private void Awake()
    {
        player = GetComponent<Transform>();
        playerBoost = GetComponent<PlayerBoost>();

        skiGameInputActions = new SkiGameInputActions();
        
        // Subscribe to new input asset SkiGameInputActions Boost action
        skiGameInputActions.GamePlay.LeftTurn.performed += ctx => TurnLeft(ctx.ReadValue<float>());
        skiGameInputActions.GamePlay.RightTurn.performed += ctx => TurnRight(ctx.ReadValue<float>());
    }


    private void Start()
    {
        // grabs references to components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        initialRotation = player.rotation;

       //playerDamage = GetComponent<PlayerDamage>();
    }


    private void OnEnable()
    {
        skiGameInputActions.Enable();
    }


    private void OnDisable()
    {
        skiGameInputActions.Disable();
    }


    private void Update()
    {
        if (isMoving) // && !playerDamage.hurt)
        {
            bool onGround = Physics.Linecast(transform.position, groundCheck.position, groundLayers);
            bool shouldPlaySkiAudio = onGround && playerStats.speed > 300f;

            animator.SetBool("grounded", onGround);
            float turnLeftValue = skiGameInputActions.GamePlay.LeftTurn.ReadValue<float>();
            float turnRightValue = skiGameInputActions.GamePlay.RightTurn.ReadValue<float>();

            if (shouldPlaySkiAudio && !isSkiAudioPlaying)
            {
                SoundManager.instance.PlaySkiing();
                isSkiAudioPlaying = true;
            }

            else if (!shouldPlaySkiAudio && isSkiAudioPlaying)
            {
                SoundManager.instance.StopSkiing();
                isSkiAudioPlaying = false;
            }

            if (onGround)
            {   
                //if (Input.GetKey(left))
                if (turnLeftValue > 0f)
                {
                    //TurnLeft();
                    if (transform.eulerAngles.y < 269)
                    {
                        transform.Rotate(new Vector3(0, playerStats.turnSpeed, 0) * Time.deltaTime, Space.Self);
                    }
                }
                
                //if (Input.GetKey(right))
                if (turnRightValue > 0f)
                {
                    //TurnRight();
                    if (transform.eulerAngles.y > 91)
                    {
                        transform.Rotate(new Vector3(0, -playerStats.turnSpeed, 0) * Time.deltaTime, Space.Self);
                    }
                }

            }

            else if (!onGround)
            {
                SoundManager.instance.PlayJumpWhoosh();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        
        ControlSpeed();

        if (isMoving) // && !playerDamage.hurt)
        {
            // increase or decrease the players speed depending on how much they are facing downhill
            float turnAngle = Mathf.Abs(180 - transform.eulerAngles.y);
            playerStats.speed += Remap(0, 90, playerStats.turnAcceleration, -playerStats.turnDeceleration, turnAngle);

            //TODO (Will): Coding note - We are in the FixedUpdate step. Multiplying by Time.fixedDeltaTime is unneccesary. Parentheses unnecessary.  This line should probably just be "Vector3 velocity = transform.forward * playerStats.speed;"
            // moves the player forward based on which direction they are facing
            Vector3 velocity = (transform.forward) * playerStats.speed * Time.fixedDeltaTime;

            if (playerBoost.IsBoostActive())
            {
                velocity += playerBoost.GetBoostVelocity();
                playerStats.speed += playerBoost.GetBoostVelocity().magnitude;
                velocity.y = rb.velocity.y;
            }

            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

        // update the Animator's state depending on our speed;
        animator.SetFloat("playerSpeed", playerStats.speed);
    }

    private void TurnLeft(float value)  // Added float value parameter
    {
        // rotates the player, limiting them after reaching a certain angle
    }

    private void TurnRight(float value)  // Added float value parameter
    {
        // rotates the player, limiting them after reaching a certain angle
    }

    private void ControlSpeed()
    {

        // limits the player's speed when reaching past the speed maximum
        if (playerStats.speed > playerStats.speedMaximum)
        {
            playerStats.speed = playerStats.speedMaximum;
        }

        // limits the player from moving any slower than the speed minimum
        if (playerStats.speed < playerStats.speedMinimum)
        {
            playerStats.speed = playerStats.speedMinimum;
            
        }

    }


    // remaps a number from a given range into a new range
    private float Remap(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
        return (NewValue);
    }

    public void ResetPlayerState()
    {
        player.rotation = initialRotation;
    }


    public IEnumerator PlayerRebound()
    {
        canMove = false;

        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.forward * 30f, ForceMode.Impulse);
        SoundManager.instance.PlayRebound();
        
        yield return new WaitForSeconds(1.5f);
         
        canMove = true;
    }

}