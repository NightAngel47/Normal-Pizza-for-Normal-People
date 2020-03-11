using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CustomerLine : MonoBehaviour
{
    private GameManager gameManager;
    private List<Order> customerOrders = new List<Order>();
    private OrderCreation orderCreation;
    [SerializeField]
    private TMP_Text currentDayCustomerText;
    private int currentDayCustomerServing;
    private int currentDayNumOfCustomers;
    private int currentAmountOfCustomersInShop;
    [SerializeField]
    private GameObject customerPrefab;
    [SerializeField]
    private Transform customerSpawnPos;
    [SerializeField]
    private List<Transform> customerLines = new List<Transform>();

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
        yield return new WaitUntil(() => currentAmountOfCustomersInShop < gameManager.gameDays[gameManager.currentDay].numOfCustomersInQue);

        if (gameManager.currentDayTimer < 1 || customerOrders.Count <= 0) yield break;
        
        currentAmountOfCustomersInShop++;
        currentDayCustomerServing++;
        currentDayCustomerText.text = currentDayCustomerServing + "/" + currentDayNumOfCustomers;
            
        var newCustomer = Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>();
        newCustomer.SetOrder(customerOrders[0]);
        newCustomer.SetTargetLine(customerLines[0].position);
        newCustomer.GetComponent<NavMeshAgent>().SetDestination(customerLines[0].position);
        customerOrders.Remove(customerOrders[0]);
        
        yield return new WaitForSeconds(gameManager.gameDays[gameManager.currentDay].dayLength /
                                        gameManager.gameDays[gameManager.currentDay].numOfCustomers);
        
        if (gameManager.currentDayTimer > 1 && customerOrders.Count > 0)
        {
            StartCoroutine(NextCustomer());
        }
    }

    public void CustomerServed()
    {
        currentAmountOfCustomersInShop--;
    }
}
