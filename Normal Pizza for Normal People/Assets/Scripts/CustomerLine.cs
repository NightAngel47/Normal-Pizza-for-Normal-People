using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CustomerLine : MonoBehaviour
{
    private GameManager gameManager;
    private List<Order> customerOrders = new List<Order>();
    private OrderCreation orderCreation;
    [SerializeField]
    private TMP_Text currentDayCustomerText;
    private int currentDayCustomerServing;
    private int currentDayNumOfCustomers;
    [SerializeField]
    private GameObject customerPrefab;
    [SerializeField]
    private Transform customerSpawnPos;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        orderCreation = GetComponent<OrderCreation>();
    }

    public void StartDay(int numOfCustomers)
    {
        customerOrders = orderCreation.GenerateOrders(numOfCustomers);
        currentDayCustomerServing = 0;
        currentDayNumOfCustomers = numOfCustomers;
        StartCoroutine(NextCustomer());
    }

    private IEnumerator NextCustomer()
    {
        if (gameManager.currentDayTimer > 1 && customerOrders.Count > 0 && !Physics.CheckSphere(customerSpawnPos.position, 0.5f))
        {
            yield return new WaitForSeconds(1f);
            currentDayCustomerServing++;
            currentDayCustomerText.text = currentDayCustomerServing + "/" + currentDayNumOfCustomers;
            
            Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>().SetOrder(customerOrders[0]);
            customerOrders.Remove(customerOrders[0]);
        }
        
        yield return new WaitForEndOfFrame();
            
        if (gameManager.currentDayTimer > 1 && customerOrders.Count > 0)
        {
            StartCoroutine(NextCustomer());
        }
    }
}
