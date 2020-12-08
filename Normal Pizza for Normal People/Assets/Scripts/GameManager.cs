/*
 * Normal Pizza for Normal People
 * IM 389
 * GameManager
 * Steven & Sydney
 * Steven:
 * Sydney: Toggle pointer function, start day button and days being started by the player, and pizza spawning button turning on and off,
 * Manages the game
 */

using System;
using System.Collections;
using System.Collections.Generic;
using ACTools.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomerLine), typeof(OrderCreation))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private CustomerLine customerLine = null;
    [SerializeField] private MoneyTracker moneyTracker = null;
    [SerializeField] private LevelManager levelManager;
    private AudioSource audioSource = null;

    [Header("Day Progress UI")]
    [SerializeField] private TMP_Text currentDayText = null;
    [SerializeField] private TMP_Text currentDayTime = null;
    [SerializeField] private Image currentDayProgressBar = null;

    [Header("Day Summary UI")]
    [SerializeField] private GameObject endOfDaySummary;
    [SerializeField] private GameObject startOfDaySummary;
    [SerializeField] private TMP_Text daySummaryTitle;
    [SerializeField] private Color missedStarColor;
    [SerializeField] private List<Image> daySummaryStars;

    [Header("Game Days")]
    [HideInInspector] public Day currentGameDay = new Day();
    [HideInInspector] public float currentDayTimer = 0f;

    public GameObject gameOverButtons = null;
    public TMP_Text gameOverText = null;
    public GameObject pointer = null;
    public GameObject inputMod = null;

    [HideInInspector] public bool dayStarted = false;
    public GameObject startDayButton = null;
    public GameObject nextLevelButton = null;
    public GameObject pizzaSpawnButton = null;

    private readonly List<Customer> remainingActiveCustomers = new List<Customer>();

    enum GameDayAudioStates {StartDay, EndDay}
    [SerializeField] List<AudioClip> gameDayAudioClips = new List<AudioClip>();

    [Serializable]
    public class Day
    {
        [Header("Day Info")]
        [Tooltip("The day number")] public int dayNum;
        [Tooltip("The number of customers per day"), Range(0, 50)] public int numOfCustomers;
        [Tooltip("The length of each day in seconds"), Range(0, 300)] public int dayLength;
        [Tooltip("The profit goal for each day")] public int moneyGoal;
        [Tooltip("The star two goal for each day")] public int starTwoGoal;
        [Tooltip("The star three goal for each day")] public int starThreeGoal;
        [Tooltip("The amount of stars earned that day")] public int starsEarned;

        public Day()
        {

        }

        public Day(int num, int customers, int length, int goal, int secondGoal, int thirdGoal)
        {
            dayNum = num;
            numOfCustomers = customers;
            dayLength = length;
            moneyGoal = goal;
            starTwoGoal = secondGoal;
            starThreeGoal = thirdGoal;
        }
    }

    private void Start()
    {
        // Setup level based on the day
        levelManager.SetupLevel(LevelSelect.selectedLevel);

        audioSource = GetComponent<AudioSource>();

        endOfDaySummary.SetActive(false);
        gameOverButtons.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        pizzaSpawnButton.SetActive(false);

        FindObjectOfType<PauseMenu>().SetUp();

        MusicManager.instance.ChangeMusic(MusicManager.MusicTrackName.BetweenDaysMusic);
    }

    public void SetDay(int num, int customers, int length, int goal, int secondGoal, int thirdGoal)
    {
        currentGameDay = new Day(num, customers, length, goal, secondGoal, thirdGoal);
    }

    public MoneyTracker GetMoneyTracker()
    {
        return moneyTracker;
    }

    public void TogglePointer(bool state)
    {
        pointer.SetActive(state);
    }

    public void ChangeDayPanels()
    {
        endOfDaySummary.SetActive(false);
        startOfDaySummary.SetActive(true);
        startDayButton.SetActive(true);
        nextLevelButton.SetActive(false);
    }

    public void StartDay()
    {
        dayStarted = true;
        startDayButton.SetActive(false);
        pizzaSpawnButton.SetActive(true);
        startOfDaySummary.SetActive(false);

        StartCoroutine(DayCycle());
    }

    private IEnumerator DayCycle()
    {
        endOfDaySummary.SetActive(false);
        daySummaryTitle.text = "Day " + currentGameDay.dayNum + " Summary";
        foreach (var star in daySummaryStars)
        {
            star.color = Color.white;
        }
        currentDayText.text = "Day " + currentGameDay.dayNum;
        currentDayTimer = currentGameDay.dayLength;
        customerLine.StartDay(currentGameDay.numOfCustomers);
        ShowHideDayTimer(true);
        StartCoroutine(DayTimer());
        moneyTracker.TrackNewDay();

        // Change to work day music
        MusicManager.instance.ChangeMusic(MusicManager.MusicTrackName.WorkDayMusic);
        audioSource.clip = gameDayAudioClips[(int) GameDayAudioStates.StartDay];
        audioSource.Play();

        // wait until end of day
        yield return new WaitUntil(() => currentDayTimer <= 0);
        
        // Keeps active customers at counter, and tells other customers in shop to leave
        foreach (var customer in FindObjectsOfType<Customer>())
        {
            if (customer.activeOrder)
            {
                remainingActiveCustomers.Add(customer);
            }

            StartCoroutine(customer.CustomerLeave());
        }

        // wait until remaining active customers are dealt with
        yield return new WaitUntil(() => remainingActiveCustomers.Count == 0);

        audioSource.clip = gameDayAudioClips[(int) GameDayAudioStates.EndDay];
        audioSource.Play();

        dayStarted = false;

        yield return new WaitForSeconds(1f);
        endOfDaySummary.SetActive(true);
        pizzaSpawnButton.SetActive(false);

        if (moneyTracker.GetCurrentDayAmount() >= currentGameDay.moneyGoal)
        {
            // increase stars earned for reaching the day's money goal
            currentGameDay.starsEarned++;
            IncreaseDayDifficulty();

            nextLevelButton.SetActive(true);
        }
        else
        {
            //TODO add game over transition
            
            daySummaryTitle.text = "Game Over";
            if (currentGameDay.dayNum == 1)
            {
                gameOverText.text = "You have completed "+ currentGameDay.dayNum +" Day\n of Pizza Research!";
            }
            else
            {
                gameOverText.text = "You have completed "+ currentGameDay.dayNum +" Days\n of Pizza Research!";
            }

            foreach (var star in daySummaryStars)
            {
                star.color = missedStarColor;
            }
            
            gameOverButtons.SetActive(true);
            gameOverText.gameObject.SetActive(true);

            foreach (var customerLinePos in FindObjectsOfType<CustomerLinePos>())
            {
                customerLinePos.gameObject.SetActive(!customerLinePos.gameObject.activeSelf);
            }

            TogglePointer(true);
        }
    }

    private IEnumerator DayTimer()
    {
        var dayTimeMin = (int) (currentDayTimer / 60);
        var dayTimeSec = (int) (currentDayTimer - (dayTimeMin * 60));
        if (dayTimeSec < 10)
        {
            currentDayTime.text = dayTimeMin + ":0" + dayTimeSec;
        }
        else
        {
            currentDayTime.text = dayTimeMin + ":" + dayTimeSec;
        }
        currentDayProgressBar.fillAmount = currentDayTimer / currentGameDay.dayLength;
        yield return new WaitForEndOfFrame();
        currentDayTimer -= Time.deltaTime;
        if (currentDayTimer > 0)
        {
            StartCoroutine(DayTimer());
        }
        else
        {
            ShowHideDayTimer(false);
        }
    }

    private void ShowHideDayTimer(bool state)
    {
        currentDayTime.transform.parent.gameObject.SetActive(state);
    }

    private void IncreaseDayDifficulty()
    {
        if (moneyTracker.GetCurrentDayAmount() >= currentGameDay.starTwoGoal)
        {
            currentGameDay.starsEarned++;
        }
        else
        {
            daySummaryStars[1].color = missedStarColor;
        }
        
        if (moneyTracker.GetCurrentDayAmount() >= currentGameDay.starThreeGoal)
        {
            currentGameDay.starsEarned++;
        }
        else
        {
            daySummaryStars[2].color = missedStarColor;
        }

        if (currentGameDay.starsEarned > LoadData.FromBinaryFile<int>("npnp", $"day_{currentGameDay.dayNum}_stars"))
        {
            SaveData.ToBinaryFile("npnp", $"day_{currentGameDay.dayNum}_stars", currentGameDay.starsEarned);
        }
        
        // progress to the next day
        LevelSelect.selectedLevel++;
        
        // updated furthest level if the next level is the furthest the player has gone
        if (LevelSelect.selectedLevel > LoadData.FromBinaryFile<int>("npnp", "FurthestLevel"))
        {
            SaveData.ToBinaryFile("npnp", "FurthestLevel", LevelSelect.selectedLevel);
        }
        
        
        // reset and setup for next level
        levelManager.ResetLevel();
        levelManager.SetupLevel(LevelSelect.selectedLevel);
    }

    public void RemoveActiveCustomer(Customer customer)
    {
        if (remainingActiveCustomers.Contains(customer))
        {
            remainingActiveCustomers.Remove(customer);
        }
    }
}
