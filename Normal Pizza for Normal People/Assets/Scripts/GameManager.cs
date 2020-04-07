﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomerLine), typeof(OrderCreation))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private CustomerLine customerLine;
    [SerializeField] private MoneyTracker moneyTracker;
    [SerializeField] private UpgradeSystem upgradeSystem;
    private AudioSource audioSource;

    [Header("Day Canvas UI")]
    [SerializeField] private TMP_Text currentDayText;
    [SerializeField] private TMP_Text currentDayTime;
    [SerializeField] private Image currentDayProgressBar;

    [Header("Game Days")] 
    [SerializeField, Tooltip("The starting values for the game days.")] private Day startingDayValues;
    [SerializeField, Tooltip("The rates the each of the day stats change by.")] private Day dayRates;
    [HideInInspector] public Day currentGameDay;
    [HideInInspector] public float currentDayTimer;
    enum GameDayAudioStates {StartDay, EndDay}
    [SerializeField] List<AudioClip> gameDayAudioClips = new List<AudioClip>();
    
    [Serializable]
    public struct Day
    {
        [Header("Day Info")] 
        [Tooltip("The day number")] public int dayNum;
        [Tooltip("The number of customers per day"), Range(0, 50)] public int numOfCustomers;
        [Tooltip("The number of "), Range(1, 4)] public int numOfCustomersInQue;
        [Tooltip("The length of each day in seconds"), Range(0, 300)] public int dayLength;
        [Tooltip("The profit goal for each day")] public int moneyGoal;
    }

    private void Start()
    {
        //moneyTracker = GetComponent<MoneyTracker>();
        //customerLine = GetComponent<CustomerLine>();
        //upgradeSystem = GetComponent<UpgradeSystem>();
        audioSource = GetComponent<AudioSource>();
        
        //TODO have player start day
        currentGameDay = startingDayValues;
        StartCoroutine(DayCycle());
    }

    public MoneyTracker GetMoneyTracker()
    {
        return moneyTracker;
    }

    private IEnumerator DayCycle()
    {
        moneyTracker.ShowHideTotalMoneyUI(false);
        currentDayText.text = "Day " + (currentGameDay.dayNum);
        customerLine.StartDay(currentGameDay.numOfCustomers);
        currentDayTimer = currentGameDay.dayLength;
        ShowHideDayTimer(true);
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
        moneyTracker.ShowHideTotalMoneyUI(true);
        
        upgradeSystem.EnterUpgradeMode();

        // TODO check for game over
        yield return new WaitWhile(upgradeSystem.GetIsUpgrading);
        StartCoroutine(DayCycle());
    }

    private IEnumerator DayTimer()
    {
        currentDayTime.text = "" + (int) currentDayTimer;
        currentDayProgressBar.fillAmount = currentDayTimer / currentGameDay.dayLength;
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
            ShowHideDayTimer(false);
        }
    }

    private void ShowHideDayTimer(bool state)
    {
        currentDayTime.transform.parent.gameObject.SetActive(state);
    }
}
