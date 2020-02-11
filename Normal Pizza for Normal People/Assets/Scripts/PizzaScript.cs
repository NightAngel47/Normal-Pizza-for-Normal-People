using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaScript : MonoBehaviour
{
    public GameObject ticket = null;

    public bool isCooked = false;
    public bool isBurnt = false;

    public List<GameObject> ingredientList = new List<GameObject>();

    private void OnDestroy()
    {
        if(ticket != null)
        {
            var ticketBox = ticket.GetComponent<OrderTicketScript>().ticketBox;
            if(ticketBox != null)
            {
                ticketBox.isOpen = true;
            }
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        ingredientList.Add(col.gameObject);
    }
}
