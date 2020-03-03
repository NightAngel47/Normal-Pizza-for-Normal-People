using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTicketScript : MonoBehaviour
{
    [HideInInspector]
    public PizzaTicketBox ticketBox;

    GameObject pizza1;
    private bool pizzaCheck = false;

    public static GameObject deliveryTicket;

    private void Update() 
    {
        //pizza1 = GameObject.FindGameObjectWithTag("deliveredPizza");

        //if(pizza1 != null)
        //{
        //    pizzaCheck = true;
        //}

        //if (pizza1 != null && pizzaCheck == true)
        //{
        //    pizza1.GetComponentInParent<PizzaScript>().ticket = gameObject;
        //    gameObject.GetComponent<LockToPoint1>().snapTo = ticketLockPoint.transform;
        //    ticketBox.isOpen = false;

        //    pizzaCheck = false;
        //}
    }

    private void OnTriggerEnter(Collider col)
    {
        // pizza ticket box 1
        if (col.gameObject.CompareTag("pizza1ticket"))
        {
            ticketBox = col.gameObject.GetComponent<PizzaTicketBox>();
            print("Start: " + ticketBox.name + " " + ticketBox.isOpen);
            if (ticketBox.isOpen)
            {
                //pizza1 = GameObject.FindGameObjectWithTag("deliveredPizza");
                //if (pizza1 != null)
                //{
                    //pizza1.GetComponentInParent<PizzaScript>().ticket = gameObject;
                    gameObject.GetComponent<LockToPoint1>().snapTo = col.gameObject.transform.GetChild(0).transform;
                    deliveryTicket = gameObject;
                    ticketBox.isOpen = false;
                    pizzaCheck = false;
                //}
            }
        }
        
        print("End: " + ticketBox.name + " " + ticketBox.isOpen);
    }

    private void OnDestroy()
    {
        if (ticketBox != null)
        {
            ticketBox.isOpen = true;
        }
    }
}
