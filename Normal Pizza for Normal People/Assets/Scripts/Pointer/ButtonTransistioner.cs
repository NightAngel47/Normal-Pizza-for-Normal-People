/*
 * Normal Pizza for Normal People
 * IM 389
 * ButtonTransistioner
 * Sydney & Steven
 * Sydney: Created initial script that handles all button functionality for main menu and pause menu
 * Steven: Added audio and resets time scale to 1
 * This is the script that handles what the buttons do when they are pressed. This is for the VR UI buttons, when clicked by a pointer.
 */

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonTransistioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public GameObject panelHandler;
    private MainMenuHandle mainMenuHandle;
    private AudioSource audioSource;

    public Color32 norm = Color.white;
    public Color32 hover = Color.gray;
    public Color32 down = Color.white;
    public Color32 disabled = Color.black;

    private Image img = null;

    [SerializeField] private bool interactable = true;

    private void Awake()
    {
        img = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        mainMenuHandle = panelHandler.GetComponent<MainMenuHandle>();
    }

    public void SetInteractable(bool state)
    {
        interactable = state;

        img.color = interactable ? norm : disabled;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) return;
        
        if (gameObject.name == "Start") //main menu
        {
            mainMenuHandle.logo.SetActive(true);
            mainMenuHandle.howToPlay.SetActive(false);
            mainMenuHandle.credits.SetActive(false);
            mainMenuHandle.levelSelectPanel.SetActive(true);
            mainMenuHandle.mainMenuPanel.SetActive(false);
        }

        if (gameObject.name == "HowToPlay") //main menu
        {
            mainMenuHandle.howToPlay.SetActive(true);
            mainMenuHandle.credits.SetActive(false);
            mainMenuHandle.logo.SetActive(false);
        }

        if (gameObject.name == "Credits") //main menu
        {
            mainMenuHandle.howToPlay.SetActive(false);
            mainMenuHandle.credits.SetActive(true);
            mainMenuHandle.logo.SetActive(false);
        }

        if (gameObject.name == "Quit") //main menu
        {
            mainMenuHandle.howToPlay.SetActive(false);
            mainMenuHandle.credits.SetActive(false);
            mainMenuHandle.logo.SetActive(false);
            Application.Quit();
        }

        if (gameObject.name == "Restart") //ingame
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Game");
        }

        if (gameObject.name == "BackToMain") //ingame
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        if (gameObject.name == "Resume") //ingame
        {
            Time.timeScale = 1;
            FindObjectOfType<PauseMenu>().PauseFunction();
        }

        if (gameObject.name == "RestartPause") //ingame
        {
            Time.timeScale = 1;
            FindObjectOfType<PauseMenu>().PauseFunction();
            SceneManager.LoadScene("Game");
        }

        if (gameObject.name == "BackToMainPause") //ingame
        {
            Time.timeScale = 1;
            FindObjectOfType<PauseMenu>().pausePanel.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }

        if (gameObject.CompareTag("level"))
        {
            LevelButtonScript selectedLevel = gameObject.GetComponent<LevelButtonScript>();
            selectedLevel.ChangeSelectedLevel();
            LevelSelect.selectedLevel = selectedLevel.GetLevelNum();
            mainMenuHandle.startLevelButton.SetActive(true);
        }

        if (gameObject.name == "Back") //back from level select to main menu
        {
            mainMenuHandle.levelSelectPanel.SetActive(false);
            mainMenuHandle.mainMenuPanel.SetActive(true);
        }

        if (gameObject.name == "StartLevel")
        {
            SceneManager.LoadScene("Game");
        }

        img.color = hover;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable) return;
        
        img.color = down;
        audioSource.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable) return;
        
        img.color = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable) return;
        
        img.color = norm;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable) return;
    }
}
