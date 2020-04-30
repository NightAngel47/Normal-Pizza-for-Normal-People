using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PauseMenu : MonoBehaviour
{
    public SteamVR_Action_Boolean pauseActionBoolean;
    public bool isPaused = false;
    
    private GameObject pausePanel;

    public void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("pauseMenu");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        gameObject.GetComponent<Canvas>().worldCamera = FindObjectOfType<Pointer>().gameObject.GetComponent<Camera>(); //GameObject.FindGameObjectWithTag("pointer").GetComponent<Camera>(); //
        //FindObjectOfType<GameManager>().GetComponent<GameManager>().TogglePointer();
    }

    void Start()
    {
       

        pauseActionBoolean.AddOnStateDownListener(PauseGame, SteamVR_Input_Sources.LeftHand);
        pauseActionBoolean.AddOnStateDownListener(PauseGame, SteamVR_Input_Sources.RightHand);
        
        pausePanel = transform.GetChild(0).gameObject;
        pausePanel.SetActive(false);
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
        Transform playerPos = FindObjectOfType<Player>().transform;
        GameManager gameManager = FindObjectOfType<GameManager>();

        pausePanel = transform.GetChild(0).gameObject;
        
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
