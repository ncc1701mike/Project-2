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
    

    private void Awake() 
    {
        crashUI.SetActive(false);
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
