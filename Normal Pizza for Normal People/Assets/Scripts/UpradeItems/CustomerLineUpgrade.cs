using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLineUpgrade : ItemUpgrades
{
    [SerializeField] private GameObject newLineUI;
    [SerializeField] private float newLineUILifetime = 2f;
    
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
            Destroy(Instantiate(newLineUI, transform.position, Quaternion.identity), newLineUILifetime);
        }
    }
}
