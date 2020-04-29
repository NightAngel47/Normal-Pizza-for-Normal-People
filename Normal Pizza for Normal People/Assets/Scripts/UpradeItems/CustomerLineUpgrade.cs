using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLineUpgrade : ItemUpgrades
{
    [SerializeField] private GameObject newLineUI;

    private GameObject lineUIInstance;
    
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
            lineUIInstance = Instantiate(newLineUI, transform.position, Quaternion.identity);
            StartCoroutine(DestroyUI());
        }
    }
    
    private IEnumerator DestroyUI()
    {
        yield return new WaitUntil(() => FindObjectOfType<GameManager>().dayStarted);
        
        Destroy(lineUIInstance);
    }
}
