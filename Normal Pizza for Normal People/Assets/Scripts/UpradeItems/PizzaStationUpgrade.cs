using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaStationUpgrade : ItemUpgrades
{
    private PizzaSpawner ps;

    protected override void ChangeMaterial(Material changeMat)
    {
        gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().material = changeMat;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMat;
    }

    public override void TurnOnUpgrade()
    {
        ps = gameObject.GetComponent<PizzaSpawner>();
        ps.enabled = !ps.enabled;
    }
}
