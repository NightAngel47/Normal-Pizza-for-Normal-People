﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private bool isUpgrading;
    [SerializeField]
    private List<GameObject> availableUpgrades = new List<GameObject>();

    public void EnterUpgradeMode()
    {
        isUpgrading = true;
        print("We upgrading boissss!");
        Invoke(nameof(TempUpgradeModeTimer), 5f);
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
