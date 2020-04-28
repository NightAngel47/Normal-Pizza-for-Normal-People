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

    void Start()
    {

        pauseActionBoolean.AddOnStateDownListener(PauseGame, leftHand);
        pauseActionBoolean.AddOnStateDownListener(PauseGame, rightHand);
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
    private void PauseFunction()
    {
        if (isPaused)
        {
            isPaused = false;
            Vector3 position = FindObjectOfType<Player>().transform.position;
            position = new Vector3(position.x, 0, position.z);
            FindObjectOfType<Player>().transform.position = position;
        }
        else
        {
            isPaused = true;
            Vector3 position = FindObjectOfType<Player>().transform.position;
            position = new Vector3(position.x, -3, position.z);
            FindObjectOfType<Player>().transform.position = position;
        }
    }
}
