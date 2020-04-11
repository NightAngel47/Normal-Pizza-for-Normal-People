﻿using System.Collections;
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
        if (gameObject.name == "Start") //main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
            SceneManager.LoadScene("Game");
        }

        if(gameObject.name == "HowToPlay") //main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(true);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
        }

        if (gameObject.name == "Credits") //main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(true);
        }

        if (gameObject.name == "Quit") //main menu
        {
            panelHandler.GetComponent<MainMenuHandle>().howToPlay.SetActive(false);
            panelHandler.GetComponent<MainMenuHandle>().credits.SetActive(false);
            Application.Quit();
        }

        if (gameObject.name == "Restart") //ingame
        {
            SceneManager.LoadScene("Game");
        }

        if (gameObject.name == "BackToMain") //ingame
        {
            SceneManager.LoadScene("MainMenu");
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
