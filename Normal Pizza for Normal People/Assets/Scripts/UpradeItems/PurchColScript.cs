using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class PurchColScript : MonoBehaviour
{
    private ItemUpgrades iu;

    private float timeTotal = 5; //time hand is needed to be in collider
    private float timeInside = 0; //time the hand has been in the collider

    private bool callOnce = false; //call the if statement once

    public Image loadingBar;
    public TextMeshProUGUI progressIndicator;

    private void Start()
    {
        iu = gameObject.GetComponentInParent<ItemUpgrades>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.GetComponentInParent<HandCollider>()) //if hand is in the collider
        {
            timeInside += Time.deltaTime; //count how long it has been in the collider

            loadingBar.fillAmount = timeInside / timeTotal; //fill the loading bar

            if (timeInside >= timeTotal && callOnce == false) //has the hand been in long enough and has this function yet to be called
            {
                callOnce = true; //make sure it cannot be called again

                timeInside = 0; //reset time
                iu.Purchase(); //purchase the upgrade
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponentInParent<HandCollider>())
        {
            //reset any variables when the hand is removed
            callOnce = false;
            timeInside = 0;
        }
    }
}
