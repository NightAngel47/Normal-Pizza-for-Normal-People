using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemUpgrades : MonoBehaviour
{
    public Material blueprintMaterial;
    public Material activeMaterial;

    public float itemCost;

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
        gameObject.SetActive(true);
        TurnOffPurchaseCollider();
        TurnOnUpgrade();
        ChangeMaterial(activeMaterial);
    }

    //toggles object functionality
    public abstract void TurnOnUpgrade();

    protected abstract void TurnOffPurchaseCollider();

    protected abstract void ChangeMaterial(Material changeMat);
}
