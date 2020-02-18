using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("Start hover button")]
    private HoverButton hoverButton;
    /// <summary>
    /// Has the player started the game
    /// </summary>
    public bool inGame = false;
    /// <summary>
    /// Is the game over?
    /// </summary>
    private bool gameOver = false;
    /// <summary>
    /// Is the game paused
    /// </summary>
    public bool isPaused = false;
    
    [Header("Order Ticket Variables")]
    [SerializeField, Tooltip("The order ticket prefab")]
    private GameObject orderTicket;
    [SerializeField, Range(0, 120), Tooltip("The rate that new orders spawn at")]
    private float orderRate = 60f;
    /// <summary>
    /// The lock poses that order tickets can spawn at
    /// </summary>
    private OrderTicketLockPosBehaviour[] orderTicketPoses = new OrderTicketLockPosBehaviour[4];
    /// <summary>
    /// The current open order position
    /// </summary>
    [HideInInspector]
    public int openOrderPos;
    
    [Header("Pizza Topping Info")]
    [Tooltip("Names for the pizza toppings")]
    public string[] toppingNames;
    [Tooltip("Icons for the pizza toppings")]
    public Sprite[] toppingIcons;
    
    [Header("Money Variables")]
    [SerializeField, Tooltip("The text that displays the money earned")] 
    private TMP_Text moneyText;
    [Range(0.1f, 5), Tooltip("The amount of money a correctly delivered pizza earns, that is multiplied by the time left on the order")]
    public float moneyBaseAmount = 1.5f;
    [Range(0.1f, 5), Tooltip("The amount of money an expired ticket loses, that is multiplied by the total start time for the order")]
    public float moneyTicketExpiredAmount = 1.5f;
    /// <summary>
    /// The amount of money the player has earned
    /// </summary>
    private int moneyEarned = 0;
    [SerializeField, Tooltip("The starting amount of money")]
    private int startingMoney = 500;
    [Header("Time Variables")]
    [SerializeField, Range(1, 10), Tooltip("The starting amount of time in minutes")]
    private float startingGameTime = 5;
    private float gameTimeLeft = 0;
    [SerializeField, Tooltip("Total time left text")] 
    private TMP_Text timeText;
    [SerializeField, Tooltip("Total time left fill bar")] 
    private Image timeFillBar;
    
    [Header("Pause Menu Stuff")]
    [SerializeField, Tooltip("List of Hover Buttons for menus")]
    private HoverButton[] pauseHoverButtons = new HoverButton[4];
    private enum PauseButtons {Resume, Restart, HowToPlay, MainMenu}
    [SerializeField, Tooltip("How To Play Screen")]
    private GameObject htpScreen;
    [SerializeField, Tooltip("Resume Screen")]
    private GameObject resumeScreen;
    
    [SerializeField, Tooltip("Reference to Left Hand")]
    private SteamVR_Input_Sources leftHand;
    [SerializeField, Tooltip("Reference to Right Hand")]
    private SteamVR_Input_Sources rightHand;

    public SteamVR_Action_Boolean pauseActionBoolean;

    [SerializeField]
    private AudioClip[] audioClips; //0 30 sec left 1 done

    private void Awake()
    {
        orderTicketPoses = FindObjectsOfType<OrderTicketLockPosBehaviour>();
    }

    private void Start()
    {

        // start button listener
        hoverButton.onButtonDown.AddListener(OnButtonDown);

        // pause menu control listeners
        pauseActionBoolean.AddOnStateDownListener(PauseGame, leftHand);
        pauseActionBoolean.AddOnStateDownListener(PauseGame, rightHand);
        
        // pause menu button listeners
        pauseHoverButtons[(int) PauseButtons.Resume].onButtonDown.AddListener(OnResumeButtonDown);
        pauseHoverButtons[(int) PauseButtons.Restart].onButtonDown.AddListener(OnRestartButtonDown);
        pauseHoverButtons[(int) PauseButtons.HowToPlay].onButtonDown.AddListener(OnHowToPlayButtonDown);
        pauseHoverButtons[(int) PauseButtons.MainMenu].onButtonDown.AddListener(OnMainMenuButtonDown);

        // sets ui
        moneyEarned = startingMoney;
        moneyText.text = "$" + moneyEarned;
        gameTimeLeft = startingGameTime * 60;
        
    }

    private void Update()
    {
        RunGame();
    }
    
    private void OnButtonDown(Hand hand)
    {
        // starts game and sets start button to false
        if (!inGame && !gameOver)
        {
            inGame = true;
            hoverButton.gameObject.transform.parent.gameObject.SetActive(false);
            StartGame();
            return;
        }

        // restart game if game is over and start button is pressed
        if (!inGame && gameOver)
        {
            // loads test scene
            SceneManager.LoadScene("TestScene");
        }
    }

    /// <summary>
    /// Starts game once the start button is pressed
    /// </summary>
    private void StartGame()
    {
        InvokeRepeating(nameof(DisplayTime), 0, 0.1f);
        StartCoroutine(SpawnNewOrder());
    }

    /// <summary>
    /// Runs game until out of time
    /// </summary>
    void RunGame()
    {
        if (!inGame && !isPaused) return;
        gameTimeLeft -= Time.deltaTime;
        
        if (!(gameTimeLeft <= 0)) return;
        GameOver();
    }

    /// <summary>
    /// Stops game and presents restart button
    /// </summary>
    void GameOver()
    {
        // change states
        inGame = false;
        gameOver = true;
        
        // stop loops
        CancelInvoke(nameof(DisplayTime));
        StopAllCoroutines();
        
        // clean up level
        foreach (var orderTicket in FindObjectsOfType(typeof(OrderTicketBehaviour)))
        {
            var ticket = (OrderTicketBehaviour) orderTicket;
            Destroy(ticket.gameObject);
        }

        foreach (var pizzaScript in FindObjectsOfType(typeof(PizzaScript)))
        {
            var pizza = (PizzaScript) pizzaScript;
            Destroy(pizza.gameObject);
        }

        foreach (var topping in GameObject.FindGameObjectsWithTag("topping"))
        {
            Destroy(topping);
        }
        
        // change start button to restart button
        var startButtonParent = hoverButton.transform.parent;
        startButtonParent.GetComponentInChildren<TMP_Text>().text = "Play Again?";
        startButtonParent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Spawns new orders at order rate if there are less than 4 orders out
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnNewOrder()
    {
        // is there any open poses
        if (openOrderPos < 4 && !isPaused)
        {
            print("here");
            // find first open pos
            foreach (var orderPos in orderTicketPoses)
            {
                print("here2");
                if (!orderPos.isOpen) continue;

                print("here3");
                // spawn at open pos
                GameObject newTicket = Instantiate(orderTicket, orderPos.transform.position + Vector3.up, Quaternion.identity);
                newTicket.GetComponent<LockToPoint1>().ChangeSnap(orderPos.transform);
                newTicket.GetComponent<OrderTicketBehaviour>().SetInitialLockPos(orderPos);

                print("here3.5");
                // update openness
                orderPos.isOpen = false;
                ++openOrderPos;
                break;
            }

            print("here4");
            // wait for time
            yield return new WaitForSeconds(orderRate);
        }
        else
        {
            print("here5");
            // wait for open spot
            yield return new WaitForSeconds(orderRate / 10);
        }

        print("here6");
        yield return StartCoroutine(SpawnNewOrder());
    }

    /// <summary>
    /// Displays the amount of total time left
    /// </summary>
    private void DisplayTime()
    {
        if (isPaused) return;
        timeFillBar.fillAmount = gameTimeLeft / (startingGameTime * 60);
        
        int timeSecCurrent = (int) gameTimeLeft;
        int timeMinCurrent = timeSecCurrent / 60;
        timeSecCurrent -= timeMinCurrent * 60;

        string currentTimeLeft;

        if(Math.Abs(gameTimeLeft - 30) < 0.1f)
        {
            timeFillBar.GetComponent<AudioSource>().clip = audioClips[0];
            timeFillBar.GetComponent<AudioSource>().Play();
        }

        if (Math.Abs(gameTimeLeft - 3) < 0.1f)
        {
            timeFillBar.GetComponent<AudioSource>().clip = audioClips[1];
            timeFillBar.GetComponent<AudioSource>().Play();
        }

        if (timeSecCurrent < 10)
        {
            currentTimeLeft = timeMinCurrent + ":" + "0" + timeSecCurrent;
        }
        else
        {
            currentTimeLeft = timeMinCurrent + ":" + timeSecCurrent;
        }

        timeText.text = currentTimeLeft;
    }

    /// <summary>
    /// Changes the total money earned for completed pizza, ruined pizzas and missed tickets
    /// </summary>
    /// <param name="amount">The amount that the total money is changed by. Value is added, so pass negative value for subtraction</param>
    public void MoneyChanged(int amount)
    {
        moneyEarned += amount;
        moneyText.text = "$" + $"{moneyEarned:n0}";
        if (moneyEarned <= 0)
        {
            GameOver();
        }
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
    
    
    private void OnResumeButtonDown(Hand hand)
    {
        htpScreen.SetActive(false);
        resumeScreen.SetActive(true);
        PauseFunction();
    }

    private void OnRestartButtonDown(Hand hand)
    {
        SceneManager.LoadScene("TestScene");
    }

    private void OnHowToPlayButtonDown(Hand hand)
    {
        htpScreen.SetActive(!htpScreen.activeSelf);
        resumeScreen.SetActive(!htpScreen.activeSelf);
    }

    private void OnMainMenuButtonDown(Hand hand)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
