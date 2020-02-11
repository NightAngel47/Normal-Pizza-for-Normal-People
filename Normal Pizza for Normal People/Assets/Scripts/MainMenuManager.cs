using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField, Tooltip("New Game Button")]
    private HoverButton newGameButton;
    [SerializeField, Tooltip("How To Play Button")]
    private HoverButton howToPlayButton;
    [SerializeField, Tooltip("Credits Button")]
    private HoverButton creditsButton;
    [SerializeField, Tooltip("Quit Game Button")]
    private HoverButton quitGameButton;

    [SerializeField, Tooltip("Start Game Screen")]
    private GameObject startGameScreen;
    [SerializeField, Tooltip("How to play Screen")]
    private GameObject htpScreen;
    [SerializeField, Tooltip("Credits Screen")]
    private GameObject creditsScreen;
    
    
    private void Start()
    {
        newGameButton.onButtonDown.AddListener(OnNewGameButtonDown);
        howToPlayButton.onButtonDown.AddListener(OnHowToPlayButtonDown);
        creditsButton.onButtonDown.AddListener(OnCreditsButtonDown);
        quitGameButton.onButtonDown.AddListener(OnQuitButtonDown);
    }
    
    IEnumerator LoadAsyc()
    {
        AsyncOperation asyncLoad = null;
        asyncLoad = SceneManager.LoadSceneAsync("TestScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnNewGameButtonDown(Hand hand)
    {
        startGameScreen.SetActive(true);
        htpScreen.SetActive(false);
        creditsScreen.SetActive(false);
        
        StartCoroutine(LoadAsyc());
    }
    
    private void OnHowToPlayButtonDown(Hand hand)
    {
        htpScreen.SetActive(!htpScreen.activeSelf);
        startGameScreen.SetActive(!htpScreen.activeSelf);
        creditsScreen.SetActive(false);
        
    }
    
    private void OnCreditsButtonDown(Hand hand)
    {
        creditsScreen.SetActive(!creditsScreen.activeSelf);
        startGameScreen.SetActive(!creditsScreen.activeSelf);
        htpScreen.SetActive(false);
    }
    
    private void OnQuitButtonDown(Hand hand)
    {
        Application.Quit();
    }
}
