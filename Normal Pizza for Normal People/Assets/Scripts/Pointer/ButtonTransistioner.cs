using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonTransistioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public GameObject panelHandler;
    private AudioSource audioSource;

    public Color32 norm = Color.white;
    public Color32 hover = Color.gray;
    public Color32 down = Color.white;

    private Image img = null;

    private void Awake()
    {
        img = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.name == "Start")
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
            SceneManager.LoadScene("Game");
        }

        if(gameObject.name == "HowToPlay")
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(true);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
        }

        if (gameObject.name == "Credits")
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(true);
        }

        if (gameObject.name == "Quit")
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
            Application.Quit();
        }
        img.color = hover;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        img.color = down;
        audioSource.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.color = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = norm;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
