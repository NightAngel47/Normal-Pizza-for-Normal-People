using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemUpgrades : MonoBehaviour
{
    public Material blueprintMaterial;
    public Material activeMaterial;

    public void ShowItem()
    {
        ChangeMaterial(blueprintMaterial);
    }

    public void HideItem()
    {

    }

    public void Purchase()
    {

    }

    protected abstract void ChangeMaterial(Material changeMat);
}
