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
        pis = gameObject.GetComponent<PizzaIngredientSpawner>();
        pis.enabled = !pis.enabled;
    }
}
