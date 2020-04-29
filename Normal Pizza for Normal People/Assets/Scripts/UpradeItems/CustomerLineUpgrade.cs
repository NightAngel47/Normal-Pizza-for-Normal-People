using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CustomerLineUpgrade : ItemUpgrades
{
    [SerializeField] private GameObject newLineUI;

    private GameObject lineUIInstance;
    
    private CustomerLinePos customerLinePos;
    private Camera vrCam;

    private void Start()
    {
        vrCam = Camera.main;
    }

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
            lineUIInstance.transform.LookAt(vrCam.transform.position + new Vector3(0, 180, 0));
            StartCoroutine(DestroyUI());
        }
    }
    
    private IEnumerator DestroyUI()
    {
        yield return new WaitUntil(() => FindObjectOfType<GameManager>().dayStarted);
        
        Destroy(lineUIInstance);
    }
}
