using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PauseMenu : MonoBehaviour
{
    //public SteamVR_Input_Sources handType;

    [SerializeField, Tooltip("Reference to Left Hand")]
    private SteamVR_Input_Sources leftHand;
    [SerializeField, Tooltip("Reference to Right Hand")]
    private SteamVR_Input_Sources rightHand;

    public SteamVR_Action_Boolean pauseActionBoolean;

    public bool isPaused = false;

    public GameObject pausePanel;

    private Transform playerPos;
    private GameManager gameManager;

    void Start()
    {
        pauseActionBoolean.AddOnStateDownListener(PauseGame, leftHand);
        pauseActionBoolean.AddOnStateDownListener(PauseGame, rightHand);
        
        pausePanel.SetActive(false);
        playerPos = FindObjectOfType<Player>().transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("up");
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("down");
    }

    /// <summary>
    /// Pause game input
    /// </summary>
    private void PauseGame(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        PauseFunction();
    }

    /// <summary>
    /// Pauses game
    /// </summary>
    public void PauseFunction()
    {
        
        if (isPaused)
        {
            isPaused = false;
            Vector3 position = playerPos.position;
            position = new Vector3(position.x, position.y, 0);
            playerPos.position = position;

            pausePanel.SetActive(false);

            gameManager.TogglePointer();

            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            Vector3 position = playerPos.position;
            position = new Vector3(position.x, position.y, 6.5f);
            playerPos.position = position;

            pausePanel.SetActive(true);

            gameManager.TogglePointer();
            
            Time.timeScale = 0;
        }
    }
}
