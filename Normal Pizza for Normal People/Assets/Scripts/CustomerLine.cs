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
using UnityEngine.AI;

public class CustomerLine : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;
    private List<Order> customerOrders = new List<Order>();
    [SerializeField] private OrderCreation orderCreation = null;
    //[SerializeField] private TMP_Text currentDayCustomerText;
    private int currentDayCustomerServed = 0;
    private int currentDayNumOfCustomers = 0;
    [SerializeField] private GameObject customerPrefab = null;
    [SerializeField] private Transform customerSpawnPos = null;
    [SerializeField] private List<CustomerLinePos> customerLines = new List<CustomerLinePos>();
    private List<Customer> customersWaiting = new List<Customer>();
    
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
        //currentDayCustomerText.text = currentDayCustomerServed + "/" + currentDayNumOfCustomers;
        StartCoroutine(NextCustomer());
    }

    private IEnumerator NextCustomer()
    {
        Customer nextCustomer = CustomerToMove();
        
        // if there is not a customer to move, spawn a new one if room in shop
        if (nextCustomer == null && customersWaiting.Count < customerLines.Count * 2)
        {
            Debug.Log("Spawning new Customer");
            
            nextCustomer = Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>();
            customersWaiting.Add(nextCustomer);
            nextCustomer.SetOrder(customerOrders[0]);
            customerOrders.Remove(customerOrders[0]);
        }
        else
        {
            Debug.Log("Moving existing Customer");
        }

        if (nextCustomer != null)
        {
            // find shortest line
            CustomerLinePos shortestLine = customerLines[0];
            foreach (var line in customerLines.Where(line => line.customersInLine <= shortestLine.customersInLine))
            {
                shortestLine = line;
            }
        
            // send customer to shortest line
            nextCustomer.SetTargetLine(shortestLine.transform.position);
            shortestLine.customersInLine++;
            
            yield return new WaitForSeconds(gameManager.currentGameDay.dayLength / gameManager.currentGameDay.numOfCustomers);
        }

        yield return new WaitForEndOfFrame();
        
        // continue only if more day/customer left
        if (gameManager.currentDayTimer > 1 && customerOrders.Count > 0)
        {
            StartCoroutine(NextCustomer());
        }
    }

    public void CustomerNotWaiting(Customer customer)
    {
        customersWaiting.Remove(customer);
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

    private Customer CustomerToMove()
    {
        return (from customer in customersWaiting from line in customerLines 
                where line.customersInLine <= 0 
                where !customer.activeOrder 
                select customer).FirstOrDefault();
    }
}