using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private TMP_Text currentDayMoneyText;
    /// <summary>
    /// Current amount of money earned each day
    /// </summary>
    private int currentDayAmount = 0;
    /// <summary>
    /// Running total amount of money earned through total run
    /// </summary>
    private int totalMoneyAmount = 0;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        ChangeMoney(0);
    }

    public void TrackNewDay()
    {
        currentDayAmount = 0;
        currentDayMoneyText.text = "$" + currentDayAmount + "/$" + gameManager.gameDays[gameManager.currentDay].moneyGoal;
    }

    /// <summary>
    /// Changes the current amount by the amount passed in.
    /// </summary>
    /// <param name="amount">The amount of money earned or spent (+  or -)</param>
    public void ChangeMoney(int amount)
    {
        currentDayAmount += amount;
        totalMoneyAmount += amount;
        currentDayMoneyText.text = "$" + currentDayAmount + "/$" + gameManager.gameDays[gameManager.currentDay].moneyGoal;
        Debug.Log("Current Day Amount of Money: " + currentDayAmount);
        Debug.Log("Current Total Amount of Money: " + totalMoneyAmount);
    }
}
