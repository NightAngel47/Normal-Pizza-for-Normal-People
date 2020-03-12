using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PizzaStationUpgrade : ItemUpgrades
{
    private HoverButton hb;
    private BoxCollider bc;
    private BoxCollider bc2;
    private BoxCollider purchCol;
    private PizzaSpawner ps;

    protected override void ChangeMaterial(Material changeMat)
    {
        gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().material = changeMat;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMat;
    }

    public override void TurnOnUpgrade()
    {
        hb = gameObject.GetComponent<HoverButton>();
        bc = gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
        bc2 = gameObject.GetComponent<BoxCollider>();
        ps = gameObject.GetComponent<PizzaSpawner>();

        hb.enabled = !hb.enabled;
        bc.enabled = !bc.enabled;
        bc2.enabled = !bc2.enabled;
        ps.dont = !ps.dont;
    }

    protected override void TurnOffPurchaseCollider()
    {
        purchCol = gameObject.transform.GetChild(3).GetComponent<BoxCollider>();

        purchCol.enabled = !purchCol.enabled;
    }
}
