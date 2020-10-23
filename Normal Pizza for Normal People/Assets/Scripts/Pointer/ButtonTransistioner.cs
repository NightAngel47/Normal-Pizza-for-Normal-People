/*
 * Normal Pizza for Normal People
 * IM 389
 * ButtonTransistioner
 * Sydney & Steven
 * Sydney: Created initial script that handles all button functionality for main menu and pause menu
 * Steven: Added audio and resets time scale to 1
 * This is the script that handles what the buttons do when they are pressed. This is for the VR UI buttons, when clicked by a pointer.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonTransistioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public GameObject panelHandler;
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
            panelHandler.GetComponent<MainMenuHandle>().logo.SetActive(true);
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().levelSelectPanel.SetActive(true);
            panelHandler.GetComponent<MainMenuHandle>().mainMenuPanel.SetActive(false);
        }

        if (gameObject.name == "HowToPlay") //main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(true);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().logo.SetActive(false);
        }

        if (gameObject.name == "Credits") //main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(true);
            panelHandler.GetComponent<MainMenuHandle>().logo.SetActive(false);
        }

        if (gameObject.name == "Quit") //main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().logo.SetActive(false);
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
            LevelSelect.selectedLevel = gameObject.GetComponent<LevelButtonScript>().GetLevelNum();
            panelHandler.GetComponent<MainMenuHandle>().startLevelButton.SetActive(true);
        }

        if (gameObject.name == "Back") //back from level select to main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().levelSelectPanel.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().mainMenuPanel.SetActive(true);
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
