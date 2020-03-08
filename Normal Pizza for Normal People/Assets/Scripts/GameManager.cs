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

    [SerializeField]
    private TMP_Text currentDayText;
    [SerializeField]
    private TMP_Text currentDayTime;
    [SerializeField]
    private Image currentDayProgressBar;
    
    [SerializeField]
    public List<Day> gameDays = new List<Day>(7);
    private int currentDay = 0;
    private float currentDayTimer;
    

    [Serializable]
    public struct Day
    {
        [Header("Day Info")]
        [Tooltip("The number of customers per day"), Range(0, 50)]
        public int numOfCustomers;
        [Tooltip("The length of each day in seconds"), Range(0, 300)]
        public int dayLength;
    }

    private void Start()
    {
        moneyTracker = new MoneyTracker();
        customerLine = GetComponent<CustomerLine>();
        //TODO have player start day
        StartCoroutine(DayCycle());
    }

    public MoneyTracker GetMoneyTracker()
    {
        return moneyTracker;
    }

    private IEnumerator DayCycle()
    {
        currentDayText.text = "Day " + currentDay + 1;
        customerLine.StartDay(gameDays[currentDay].numOfCustomers);
        currentDayTimer = gameDays[currentDay].dayLength;
        StartCoroutine(DayTimer());
        
        //TODO add upgrade pause
        
        yield return new WaitForSeconds(gameDays[currentDay].dayLength);
        currentDay++;
        StartCoroutine(DayCycle());
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
    }
}
