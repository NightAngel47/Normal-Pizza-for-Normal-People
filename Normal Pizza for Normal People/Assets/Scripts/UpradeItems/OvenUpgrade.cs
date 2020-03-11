using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenUpgrade : ItemUpgrades
{
    private OvenBehaviour ob;

    protected override void ChangeMaterial(Material changeMat)
    {
        gameObject.GetComponent<MeshRenderer>().material = changeMat;
    }

    public override void TurnOnUpgrade()
    {
        ob = gameObject.transform.GetChild(0).GetComponent<OvenBehaviour>();

        ob.enabled = !ob.enabled;
    }
}
