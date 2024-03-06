using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{

     [Tooltip("Score UI.")]
    public GameObject scoreUI;

    [Tooltip("Player Speed UI.")]
    public GameObject speedUI;

    [Tooltip("Player Crash UI.")]
    public GameObject crashUI;

    [Tooltip ("Race Timer UI.")]
    public GameObject raceTimerUI;

    [Tooltip("Crash text.")]
    public Text crashText;
    
    [Tooltip("Score text.")]
    public Text scoreText;

    [Tooltip("Speed text.")]
    public Text speedText;

    [Tooltip("RaceTimer text.")]
    public Text raceTimerText;

    public PlayerController playerController;
    public Flags flags;
    public static GameManager instance;
    

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        crashUI.SetActive(false);
        raceTimerUI.SetActive(false);

        AssignPlayerController();
    }

    private void Start()
    {
        flags = FindFirstObjectByType<Flags>();  
    }


    private void OnEnable()
    {
       GameObstacle.OnPlayerCollide += HandlePlayerRebound;
       SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameObstacle.OnPlayerCollide -= HandlePlayerRebound;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayerController();
    }

    private void AssignPlayerController()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned after scene load.");
        }
    }

    private void HandlePlayerRebound(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        StartCoroutine(playerController.PlayerRebound());
    }

    void Update()
    {
       if (flags != null)
       {
            float raceTime = flags.GetTimeSinceRaceStart();
            raceTimerText.text = ("RaceTimer: " + raceTime.ToString("F2") + " seconds");
       } 
        
       
       
        if (playerController != null)
        {
            speedText.text = ("Speed: " + playerController.playerStats.speed.ToString("0"));
            scoreText.text = ("Score: " + ScoreManager.score.ToString("0"));
        }

        else
        {
            Debug.LogError("PlayerController is not assigned");
        }

    }


    public void ShowRaceTimer()
    {
        raceTimerUI.SetActive(true);
    }

}
