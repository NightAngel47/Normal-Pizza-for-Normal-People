using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUpgrade : ItemUpgrades
{
    public override void TurnOnUpgrade()
    {
        gameObject.SetActive(true);
    }

    protected override void ChangeMaterial(Material changeMat)
    {
        Debug.Log("");
    }

    protected override void TurnOffPurchaseCollider()
    {
        Debug.Log("");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
