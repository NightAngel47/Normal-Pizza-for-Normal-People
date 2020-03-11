﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private bool isUpgrading;
    [SerializeField]
    private List<GameObject> availableUpgrades = new List<GameObject>();

    public void Start()
    {
        foreach (GameObject g in availableUpgrades)
        {
            g.GetComponent<ItemUpgrades>().TurnOnUpgrade();
            g.GetComponent<ItemUpgrades>().HideItem();
        }
    }

    public void EnterUpgradeMode()
    {
        isUpgrading = true;
        print("We upgrading boissss!");

        foreach(GameObject g in availableUpgrades)
        {
            g.GetComponent<ItemUpgrades>().ShowItem();
        }

        //Invoke(nameof(TempUpgradeModeTimer), 5f);
    }

    private void TempUpgradeModeTimer()
    {
        isUpgrading = false;
    }
    
    public bool GetIsUpgrading()
    {
        return isUpgrading;
    }
}
