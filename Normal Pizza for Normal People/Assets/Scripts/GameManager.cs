using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CustomerLine customerLine;
    private MoneyTracker moneyTracker;

    private void Start()
    {
        moneyTracker = new MoneyTracker();
        customerLine = GetComponent<CustomerLine>();
    }

    public MoneyTracker GetMoneyTracker()
    {
        return moneyTracker;
    }

    private void WorkDay()
    {
        customerLine.StartDay(10);
    }
}
