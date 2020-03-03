using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerLine : MonoBehaviour
{
    private List<Order> customerOrders = new List<Order>();
    private OrderCreation orderCreation;
    [SerializeField]
    private GameObject customerPrefab;
    [SerializeField]
    private Transform customerSpawnPos;

    void Start()
    {
        orderCreation = GetComponent<OrderCreation>();
    }

    public void StartDay(int numOfCusomers)
    {
        customerOrders = orderCreation.GenerateOrders(numOfCusomers);
        NextCustomer();
    }

    public void NextCustomer()
    {
        Customer newCustomer = Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>();
        newCustomer.SetOrder(customerOrders.First());
        customerOrders.Remove(customerOrders.First());
    }
}
