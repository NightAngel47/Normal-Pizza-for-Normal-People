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
    private int currentAmount = 0;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    /// <summary>
    /// Changes the current amount by the amount passed in.
    /// </summary>
    /// <param name="amount">The amount of money earned or spent (+  or -)</param>
    public void ChangeMoney(int amount)
    {
        currentAmount += amount;
        currentDayMoneyText.text = "$" + currentAmount + "/$" + gameManager.gameDays[gameManager.currentDay].moneyGoal;
        Debug.Log("Current Amount of Money: " + currentAmount);
    }
}
