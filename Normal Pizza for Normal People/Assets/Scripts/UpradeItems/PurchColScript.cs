﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class PurchColScript : MonoBehaviour
{
    private ItemUpgrades iu;

    private float timeTotal = 5;
    private float timeInside = 0;

    private bool callOnce = false;

    public Image loadingBar;
    public TextMeshProUGUI progressIndicator;

    private void Start()
    {
        iu = gameObject.GetComponentInParent<ItemUpgrades>();
    }

    private void OnTriggerEnter(Collider col)
    {
        //if (col.GetComponentInParent<HandCollider>())
        //{
        //    iu.Purchase();
        //}
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.GetComponentInParent<HandCollider>())
        {
            timeInside += Time.deltaTime;

            loadingBar.fillAmount = timeInside / timeTotal;

            if (timeInside >= timeTotal && callOnce == false)
            {
                callOnce = true;

                timeInside = 0;
                iu.Purchase();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        timeInside = 0;
    }
}
