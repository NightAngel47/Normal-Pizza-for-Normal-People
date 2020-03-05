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

    public void StartDay(int numOfCustomers)
    {
        customerOrders = orderCreation.GenerateOrders(numOfCustomers);
        StartCoroutine(NextCustomer());
    }

    private IEnumerator NextCustomer()
    {
        if (customerOrders.Count > 0 && !Physics.CheckSphere(customerSpawnPos.position, 0.5f))
        {
            yield return new WaitForSeconds(1f);
            
            Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>().SetOrder(customerOrders[0]);
            customerOrders.Remove(customerOrders[0]);
        }
        
        yield return new WaitForEndOfFrame();
            
        if (customerOrders.Count > 0)
        {
            StartCoroutine(NextCustomer());
        }
    }
}
