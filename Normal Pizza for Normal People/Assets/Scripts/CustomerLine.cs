using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CustomerLine : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private List<Order> customerOrders = new List<Order>();
    [SerializeField] private OrderCreation orderCreation;
    [SerializeField] private TMP_Text currentDayCustomerText;
    private int currentDayCustomerServed;
    private int currentDayNumOfCustomers;
    private int currentAmountOfCustomersInShop;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform customerSpawnPos;
    [SerializeField] private List<CustomerLinePos> customerLines = new List<CustomerLinePos>();

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        orderCreation = GetComponent<OrderCreation>();
    }

    public void StartDay(int numOfCustomers)
    {
        customerOrders = orderCreation.GenerateOrders(numOfCustomers);
        currentDayCustomerServed = 0;
        currentDayNumOfCustomers = numOfCustomers;
        currentDayCustomerText.text = currentDayCustomerServed + "/" + currentDayNumOfCustomers;
        StartCoroutine(NextCustomer());
    }

    private IEnumerator NextCustomer()
    {
        yield return new WaitUntil(() => currentAmountOfCustomersInShop < gameManager.currentGameDay.numOfCustomersInQue);

        if (gameManager.currentDayTimer < 1 || customerOrders.Count <= 0) yield break;
        
        currentAmountOfCustomersInShop++;
            
        var newCustomer = Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>();
        newCustomer.SetOrder(customerOrders[0]);
        customerOrders.Remove(customerOrders[0]);

        foreach (var line in customerLines)
        {
            if (line.isOpen)
            {
                line.isOpen = false;
                newCustomer.SetTargetLine(customerLines[customerLines.IndexOf(line)].transform.position);
                newCustomer.GetComponent<NavMeshAgent>().SetDestination(customerLines[customerLines.IndexOf(line)].transform.position);
                break;
            }
            
            if(line == customerLines[customerLines.Count - 1])
            {
                newCustomer.SetTargetLine(customerLines[Random.Range(0, customerLines.Count)].transform.position);
                newCustomer.GetComponent<NavMeshAgent>().SetDestination(customerLines[Random.Range(0, customerLines.Count)].transform.position);
                break;
            }
        }
        
        
        
        yield return new WaitForSeconds(gameManager.currentGameDay.dayLength / gameManager.currentGameDay.numOfCustomers);
        
        if (gameManager.currentDayTimer > 1 && customerOrders.Count > 0)
        {
            StartCoroutine(NextCustomer());
        }
    }

    public void IncreaseCustomersServed()
    {
        currentDayCustomerServed++;
        currentDayCustomerText.text = currentDayCustomerServed + "/" + currentDayNumOfCustomers;
    }
    
    public void CustomerServed()
    {
        currentAmountOfCustomersInShop--;
        if (currentAmountOfCustomersInShop < 0)
        {
            currentAmountOfCustomersInShop = 0;
        }
    }

    public void AddNewCustomerLine(CustomerLinePos customerLine)
    {
        customerLines.Add(customerLine);
    }
}