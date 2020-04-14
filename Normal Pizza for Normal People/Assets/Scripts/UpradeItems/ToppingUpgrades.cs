using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingUpgrades : ItemUpgrades
{
    private PizzaIngredientSpawner pis;

    protected override void ChangeMaterial(Material changeMat)
    {
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMat;
    }

    public override void TurnOnUpgrade()
    {
        isTopping = true;
        pis = gameObject.GetComponent<PizzaIngredientSpawner>();
        pis.enabled = !pis.enabled;
    }

    protected override void TurnOffPurchaseCollider()
    {
        throw new System.NotImplementedException();
    }
}
