/*
 * Normal Pizza for Normal People
 * IM 389
 * CustomerLine
 * Steven:
 * Handles sending customers to customer lines
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerLine : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private OrderCreation orderCreation = null;
    [SerializeField] private GameObject customerPrefab = null;
    [SerializeField] private Transform customerSpawnPos = null;
    [SerializeField] private List<CustomerLinePos> customerLines = new List<CustomerLinePos>();
    
    private List<Order> customerOrders = new List<Order>();
    private readonly List<Customer> customersWaiting = new List<Customer>();
    private int currentDayCustomerServed;
    
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        orderCreation = GetComponent<OrderCreation>();
    }

    public void StartDay(int numOfCustomers)
    {
        customerOrders = orderCreation.GenerateOrders(numOfCustomers);
        StartCoroutine(NextCustomer());
    }

    private IEnumerator NextCustomer()
    {
        Customer nextCustomer = null;
        
        // if there is not a customer to move, spawn a new one if room in shop
        yield return new WaitUntil(() => customersWaiting.Count < customerLines.Count * 3);
        
        nextCustomer = Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>();
        customersWaiting.Add(nextCustomer);
        nextCustomer.SetOrder(customerOrders[0]);
        customerOrders.Remove(customerOrders[0]);
        
        // find shortest line
        CustomerLinePos shortestLine = FindShortestCustomerLine();
        
        // send customer to shortest line
        nextCustomer.SetTargetLine(shortestLine);
        shortestLine.customersInLine.Add(nextCustomer);
            
        yield return new WaitForSeconds(gameManager.currentGameDay.dayLength / gameManager.currentGameDay.numOfCustomers);
        
        // continue only if more day/customer left
        if (gameManager.currentDayTimer > 1 && customerOrders.Count > 0)
        {
            StartCoroutine(NextCustomer());
        }
    }
    
    public void CustomerServed(Customer customer)
    {
        // Remove customers that where given pizzas before giving orders
        if(customersWaiting.Contains(customer))
            customersWaiting.Remove(customer);
        
        currentDayCustomerServed++;
    }

    public void AddNewCustomerLine(CustomerLinePos customerLine)
    {
        customerLines.Add(customerLine);
    }

    public CustomerLinePos FindShortestCustomerLine()
    {
        CustomerLinePos shortestLine = customerLines[0];
        foreach (var line in customerLines.Where(line => line.customersInLine.Count <= shortestLine.customersInLine.Count))
        {
            shortestLine = line;
        }

        return shortestLine;
    }
}