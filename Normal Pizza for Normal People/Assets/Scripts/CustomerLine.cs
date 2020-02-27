using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLine : MonoBehaviour
{
    private List<Order> customerOrders = new List<Order>();
    private OrderCreation orderCreation;
    [SerializeField]
    private GameObject customerPrefab;

    void Start()
    {
        orderCreation = new OrderCreation();
    }

    public void StartDay(int numOfCusomers)
    {
        customerOrders = orderCreation.GenerateOrders(numOfCusomers);
    }

    public void NextCustomer()
    {
        
    }
}
