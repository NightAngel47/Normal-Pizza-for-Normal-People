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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextCustomer();
        }
    }

    public void StartDay(int numOfCustomers)
    {
        customerOrders = orderCreation.GenerateOrders(numOfCustomers);
    }

    public void NextCustomer()
    {
        if (customerOrders.Count <= 0) return;
        Customer newCustomer = Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>();
        newCustomer.SetOrder(customerOrders[0]);
        customerOrders.Remove(customerOrders[0]);
    }
}
