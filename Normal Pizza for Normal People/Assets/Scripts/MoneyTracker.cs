using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTracker
{
    private int currentAmount;

    public MoneyTracker() => currentAmount = 0;

    public MoneyTracker(int startingAmount) => currentAmount = startingAmount;

    /// <summary>
    /// Changes the current amount by the amount passed in.
    /// </summary>
    /// <param name="amount">The amount of money earned or spent (+  or -)</param>
    public void ChangeMoney(int amount)
    {
        currentAmount += amount;
    }
}
