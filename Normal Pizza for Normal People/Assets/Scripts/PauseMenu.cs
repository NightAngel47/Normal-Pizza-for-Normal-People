/*
 * Normal Pizza for Normal People
 * IM 389
 * PauseMenu
 * Sydney 
 * Sets up the function to turn on or off the pause menu. Sets it up with the VR system so that the VR controllers can pause the game.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PauseMenu : MonoBehaviour
{
    public SteamVR_Action_Boolean pauseActionBoolean;
    public bool isPaused = false;
    
    public GameObject pausePanel;

    public void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("pauseMenu");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        pauseActionBoolean.AddOnStateDownListener(PauseGame, SteamVR_Input_Sources.LeftHand);
        pauseActionBoolean.AddOnStateDownListener(PauseGame, SteamVR_Input_Sources.RightHand);
        
        pausePanel = transform.GetChild(0).gameObject;
        pausePanel.SetActive(false);
    }

    public void SetUp()
    {
        gameObject.GetComponent<Canvas>().worldCamera = FindObjectOfType<Pointer>().gameObject.GetComponent<Camera>(); //GameObject.FindGameObjectWithTag("pointer").GetComponent<Camera>(); //
        FindObjectOfType<GameManager>().TogglePointer(false);
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
        if (SceneManager.GetActiveScene().name == "Game")
        {
            PauseFunction();
        }
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

            gameManager.TogglePointer(false);

            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            Vector3 position = playerPos.position;
            position = new Vector3(position.x, position.y, 6.5f);
            playerPos.position = position;

            pausePanel.SetActive(true);

            gameManager.TogglePointer(true);
            
            Time.timeScale = 0;
        }
    }
}
