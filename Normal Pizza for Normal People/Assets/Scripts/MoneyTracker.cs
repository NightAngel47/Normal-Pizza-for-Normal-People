using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text currentDayMoneyText;

    [SerializeField] private List<TMP_Text> endOfDaySummaryTexts;
    private enum SummaryText
    {
        CurrentDay,
        PreviousTotal,
        NewTotal
    };
    
    /// <summary>
    /// Current amount of money earned each day
    /// </summary>
    private int currentDayAmount = 0;
    /// <summary>
    /// Running total amount of money earned through total run
    /// </summary>
    private int totalMoneyAmount = 0;

    [Header("Pizza Profit Variables")]
    public float basePizzaProfit = 25;
    public float tier1Toppings = 2f;
    public float tier2Toppings = 4f;
    public float tier3Toppings = 6f;
    public float cookedBonus = 10f;
    public float sauceBonus = 15f;
    public float cheeseBonus = 20f;

    private void Start()
    {
        CustomerChangeMoney(0);
    }

    public void TrackNewDay()
    {
        currentDayAmount = 0;
        endOfDaySummaryTexts[(int) SummaryText.PreviousTotal].text = "$" + totalMoneyAmount;
        currentDayMoneyText.text = "$" + currentDayAmount + "/$" + gameManager.currentGameDay.moneyGoal;
    }

    /// <summary>
    /// Changes the current amount by the amount passed in.
    /// </summary>
    /// <param name="amount">The amount of money earned or spent (+  or -)</param>
    public void CustomerChangeMoney(int amount)
    {
        currentDayAmount += amount;
        totalMoneyAmount += amount;
        currentDayMoneyText.text = "$" + currentDayAmount + "/$" + gameManager.currentGameDay.moneyGoal;
        endOfDaySummaryTexts[(int) SummaryText.CurrentDay].text = "$" + currentDayAmount;
        endOfDaySummaryTexts[(int) SummaryText.NewTotal].text = "$" + totalMoneyAmount;
    }

    public int GetCurrentDayAmount()
    {
        return currentDayAmount;
    }

    public bool Purchase(int amount)
    {
        if (amount > totalMoneyAmount) return false;
        totalMoneyAmount -= amount;
        Debug.Log("Current Total Amount of Money: " + totalMoneyAmount);
        return true;
    }
}
