using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLineUpgrade : ItemUpgrades
{
    private CustomerLinePos customerLinePos;

    private void Awake()
    {
        customerLinePos = GetComponent<CustomerLinePos>();
    }

    public override void TurnOnUpgrade()
    {
        customerLinePos.enabled = !customerLinePos.enabled;
        if (customerLinePos.enabled)
        {
            FindObjectOfType<CustomerLine>().AddNewCustomerLine(GetComponent<CustomerLinePos>());
        }
    }

    protected override void ChangeMaterial(Material changeMat)
    {
        Debug.Log("");
    }

    protected override void TurnOffPurchaseCollider()
    {
        Debug.Log("");
    }
}
