using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PizzaStationUpgrade : ItemUpgrades
{
    private HoverButton hb;
    private BoxCollider bc;
    private BoxCollider bc2;

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

        hb.enabled = !hb.enabled;
        bc.enabled = !bc.enabled;
        bc2.enabled = !bc2.enabled;
    }
}
