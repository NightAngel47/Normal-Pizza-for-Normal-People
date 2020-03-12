using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemUpgrades : MonoBehaviour
{
    public Material blueprintMaterial;
    public Material activeMaterial;

    private MoneyTracker mt;

    public int itemCost;

    private void Start()
    {
        mt = GameObject.FindObjectOfType<MoneyTracker>();
    }

    public void ShowItem()
    {
        gameObject.SetActive(true);
        ChangeMaterial(blueprintMaterial);
    }

    public void HideItem()
    {
        gameObject.SetActive(false); ;
    }

    public void Purchase()
    {
        if (mt.Purchase(itemCost))
        {
            gameObject.SetActive(true);
            TurnOffPurchaseCollider();
            TurnOnUpgrade();
            ChangeMaterial(activeMaterial);
        }

        else
        {
            Debug.Log("Cannot Buy Not Enough Mons");
        }
    }

    //toggles object functionality
    public abstract void TurnOnUpgrade();

    protected abstract void TurnOffPurchaseCollider();

    protected abstract void ChangeMaterial(Material changeMat);
}
