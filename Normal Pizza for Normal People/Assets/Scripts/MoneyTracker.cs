﻿/*
 * Normal Pizza for Normal People
 * IM 389
 * MoneyTracker
 * Steven & Sydney
 * Steven: Created initial script which handles the player's money earned
 * Sydney: Created purchase functionality
 * Handles the money that the player earns each day and in total
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MoneyTracker : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;
    
    [Header("Day Progress UI")]
    [SerializeField] private TMP_Text currentDayMoneyText = null;
    [SerializeField] private TMP_Text starGoalTwoText;
    [SerializeField] private TMP_Text starGoalThreeText;

    [Header("Day Summary UI")]
    [SerializeField] private TMP_Text daySummaryScoreText;
    
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
    public float doughBonus = 10f;
    public float cheeseBonus = 20f;

    private void Start()
    {
        CustomerChangeMoney(0);
    }

    public void TrackNewDay()
    {
        currentDayAmount = 0;
        currentDayMoneyText.text = "$" + currentDayAmount + " / $" + gameManager.currentGameDay.moneyGoal;
        starGoalTwoText.text = "$" + currentDayAmount + " / $" + gameManager.currentGameDay.starTwoGoal;
        starGoalThreeText.text = "$" + currentDayAmount + " / $" + gameManager.currentGameDay.starThreeGoal;
    }

    /// <summary>
    /// Changes the current amount by the amount passed in.
    /// </summary>
    /// <param name="amount">The amount of money earned or spent (+  or -)</param>
    public void CustomerChangeMoney(int amount)
    {
        currentDayAmount += amount;
        totalMoneyAmount += amount;
        currentDayMoneyText.text = "$" + currentDayAmount + " / $" + gameManager.currentGameDay.moneyGoal;
        starGoalTwoText.text = "$" + currentDayAmount + " / $" + gameManager.currentGameDay.starTwoGoal;
        starGoalThreeText.text = "$" + currentDayAmount + " / $" + gameManager.currentGameDay.starThreeGoal;
        daySummaryScoreText.text = "$" + currentDayAmount;
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
