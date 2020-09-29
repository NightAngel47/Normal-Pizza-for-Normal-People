/*
 * Normal Pizza for Normal People
 * IM 389
 * UpgradeSystem
 * Sydney & Steven 
 * Sydney: Created functionality for handling available upgrades
 * Steven: Setup initial script tie in with game manager that handled when the upgrade system was upgrading
 * Sets up the upgrade system, when it starts and when it ends. Calls on the item upgrade for the respective upgrades that need to be turned on that day.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private bool isUpgrading;

    [SerializeField] private List<GameObject> availableUpgrades = new List<GameObject>();
    //[SerializeField] private int upgradeTracker = 0; //keeps track of what day and whether it is 1 or 2 upgrades

    public void Start()
    {
        foreach (GameObject g in availableUpgrades)
        {
            //make sure items do not have functionality in upgrade menu
            if (g.TryGetComponent(out ItemUpgrades upgrade) && !upgrade.TryGetComponent(out ToppingUpgrades topping))
            {
                upgrade.TurnOnUpgrade();
                upgrade.HideItem();
            }
        }
    }

    public void EnterUpgradeMode()
    {
        // Change to between days music
        MusicManager.instance.ChangeMusic(MusicManager.MusicTrackName.BetweenDaysMusic);
        isUpgrading = true;
        //print("We upgrading boissss!");

        if(availableUpgrades.Count > 1)
        {
            availableUpgrades[0].GetComponent<ItemUpgrades>().Purchase();
            availableUpgrades[0].GetComponent<ItemUpgrades>().Purchase();
        }

        //IF WE WANT TO HAVE IT DO ONE OR TWO UPGRADES
        //if (upgradeTracker == 0)
        //{
        //    availableUpgrades[0].GetComponent<ItemUpgrades>().Purchase();
        //}

        //if(upgradeTracker == 1)
        //{
        //    availableUpgrades[0].GetComponent<ItemUpgrades>().Purchase();
        //    availableUpgrades[0].GetComponent<ItemUpgrades>().Purchase();
        //}

        //upgradeTracker++;
        StartCoroutine(EndUp());

        //PLAYER CHOOSE UPGRADE LEFTOVER CODE
        //endUpgradeButton.SetActive(true);

        //foreach(GameObject g in availableUpgrades)
        //{
        //    g.GetComponent<ItemUpgrades>().ShowItem();
        //}
    }

    IEnumerator EndUp()
    {
        yield return new WaitForSeconds(1);
        isUpgrading = false;
    }

    public void RemovedPurchasedUpgrade(GameObject upgradePurchased)
    {
        availableUpgrades.Remove(upgradePurchased);
    }
    
    public bool GetIsUpgrading()
    {
        return isUpgrading;
    }
}
