using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomerLine), typeof(OrderCreation))]
public class GameManager : MonoBehaviour
{
    private CustomerLine customerLine;
    private MoneyTracker moneyTracker;
    private UpgradeSystem upgradeSystem;
    private AudioSource audioSource;

    [Header("Day Canvas UI")]
    [SerializeField]
    private TMP_Text currentDayText;
    [SerializeField]
    private TMP_Text currentDayTime;
    [SerializeField]
    private Image currentDayProgressBar;
    
    [Header("Game Days")]
    [SerializeField]
    public List<Day> gameDays = new List<Day>(7);
    [HideInInspector]
    public int currentDay = 0;
    [HideInInspector]
    public float currentDayTimer;
    enum GameDayAudioStates {StartDay, EndDay}
    [SerializeField] List<AudioClip> gameDayAudioClips = new List<AudioClip>();
    
    [Serializable]
    public struct Day
    {
        [Header("Day Info")]
        [Tooltip("The number of customers per day"), Range(0, 50)]
        public int numOfCustomers;
        [Tooltip("The number of "), Range(1, 4)]
        public int numOfCustomersInQue;
        [Tooltip("The length of each day in seconds"), Range(0, 300)]
        public int dayLength;
        [Tooltip("The profit goal for each day")]
        public int moneyGoal;
    }

    private void Start()
    {
        moneyTracker = GetComponent<MoneyTracker>();
        customerLine = GetComponent<CustomerLine>();
        upgradeSystem = GetComponent<UpgradeSystem>();
        audioSource = GetComponent<AudioSource>();
        
        //TODO have player start day
        StartCoroutine(DayCycle());
    }

    public MoneyTracker GetMoneyTracker()
    {
        return moneyTracker;
    }

    private IEnumerator DayCycle()
    {
        currentDayText.text = "Day " + (currentDay + 1);
        customerLine.StartDay(gameDays[currentDay].numOfCustomers);
        currentDayTimer = gameDays[currentDay].dayLength;
        StartCoroutine(DayTimer());
        moneyTracker.TrackNewDay();

        // Change to work day music
        MusicManager.instance.ChangeMusic(MusicManager.MusicTrackName.WorkDayMusic);
        audioSource.clip = gameDayAudioClips[(int) GameDayAudioStates.StartDay];
        audioSource.Play();
        
        yield return new WaitUntil(() => currentDayTimer <= 0);
        Customer lastCustomer = null;
        foreach (var customer in FindObjectsOfType<Customer>())
        {
            if (customer.activeOrder)
            {
                lastCustomer = customer;
            }

            customer.CustomerLeave();
        }

        if (lastCustomer != null)
        {
            yield return new WaitWhile(() => lastCustomer.activeOrder);
        }
        
        upgradeSystem.EnterUpgradeMode();
        currentDay++;
        //TODO add upgrade pause

        if (currentDay < gameDays.Count)
        {
            yield return new WaitWhile(upgradeSystem.GetIsUpgrading);
            StartCoroutine(DayCycle());
        }
    }

    private IEnumerator DayTimer()
    {
        currentDayTime.text = "" + (int) currentDayTimer;
        currentDayProgressBar.fillAmount = currentDayTimer / gameDays[currentDay].dayLength;
        yield return new WaitForEndOfFrame();
        currentDayTimer -= Time.deltaTime;
        if (currentDayTimer > 0)
        {
            StartCoroutine(DayTimer());
        }
        else
        {
            audioSource.clip = gameDayAudioClips[(int) GameDayAudioStates.EndDay];
            audioSource.Play();
        }
    }
}
