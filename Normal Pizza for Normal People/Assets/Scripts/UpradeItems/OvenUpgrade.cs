using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenUpgrade : ItemUpgrades
{
    private OvenBehaviour ob;
    private BoxCollider bc;
    private BoxCollider purchCol;

    protected override void ChangeMaterial(Material changeMat)
    {
        gameObject.GetComponent<MeshRenderer>().material = changeMat;
    }

    public override void TurnOnUpgrade()
    {
        ob = gameObject.transform.GetChild(0).GetComponent<OvenBehaviour>();
        bc = gameObject.transform.GetChild(0).GetComponent<BoxCollider>();

        ob.enabled = !ob.enabled;
        bc.enabled = !bc.enabled;
    }

    protected override void TurnOffPurchaseCollider()
    {
        purchCol = gameObject.transform.GetChild(3).GetComponent<BoxCollider>();

        purchCol.enabled = !purchCol.enabled;
    }
}
