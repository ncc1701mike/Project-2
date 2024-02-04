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

    [Tooltip("Crash text.")]
    public Text crashText;
    
    [Tooltip("Score text.")]
    public Text scoreText;

    [Tooltip("Speed text.")]
    public Text speedText;
    public PlayerMover playerMover;
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

        AssignPlayerMover();
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
        AssignPlayerMover();
    }

    private void AssignPlayerMover()
    {
        playerMover = FindFirstObjectByType<PlayerMover>();
        if (playerMover == null)
        {
            Debug.LogError("PlayerMover is not assigned after scene load.");
        }
    }

    private void HandlePlayerRebound(GameObject player)
    {
        PlayerMover playerMover = player.GetComponent<PlayerMover>();
        StartCoroutine(playerMover.PlayerRebound());
    }

    // Update is called once per frame
    void Update()
    {
        // displays the player's current score and health onto text
        if (playerMover != null)
        {
            speedText.text = ("Speed: " + playerMover.currentSpeed.ToString("0"));
            scoreText.text = ("Score: " + ScoreManager.score.ToString("0"));
        }
        else
        {
            Debug.LogError("PlayerMover is not assigned");
        }

    }

}
