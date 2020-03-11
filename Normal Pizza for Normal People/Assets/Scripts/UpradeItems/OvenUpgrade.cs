using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenUpgrade : ItemUpgrades
{
    protected override void ChangeMaterial(Material changeMat)
    {
        gameObject.GetComponent<MeshRenderer>().material = changeMat;
    }
}
