using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public GameObject endUpgradeButton;

    private bool isUpgrading;

    [SerializeField]
    private List<GameObject> availableUpgrades = new List<GameObject>();

    public void Start()
    {
        foreach (GameObject g in availableUpgrades)
        {
            //make sure items do not have functionality in upgrade menu
            g.GetComponent<ItemUpgrades>().TurnOnUpgrade();
            g.GetComponent<ItemUpgrades>().HideItem();
        }
    }

    public void EnterUpgradeMode()
    {
        // Change to between days music
        MusicManager.instance.ChangeMusic(MusicManager.MusicTrackName.BetweenDaysMusic);
        isUpgrading = true;
        print("We upgrading boissss!");

        availableUpgrades[0].GetComponent<ItemUpgrades>().Purchase();

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
        yield return new WaitForSeconds(5);
        isUpgrading = false;
    }

    public void EndUpgrade()
    {


        //PLAYER CHOOSE UPGRADE LEFTOVER CODE
        //isUpgrading = false;

        //endUpgradeButton.SetActive(false);

        //foreach (GameObject g in availableUpgrades)
        //{
        //    g.GetComponent<ItemUpgrades>().HideItem();
        //}
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
