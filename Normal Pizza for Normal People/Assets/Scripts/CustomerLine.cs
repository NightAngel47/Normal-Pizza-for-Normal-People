using System.Collections;
using System.Collections.Generic;
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
    private int currentAmountOfCustomersInShop = 0;
    [SerializeField] private GameObject customerPrefab = null;
    [SerializeField] private Transform customerSpawnPos = null;
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
        //currentDayCustomerText.text = currentDayCustomerServed + "/" + currentDayNumOfCustomers;
        StartCoroutine(NextCustomer());
    }

    private IEnumerator NextCustomer()
    {
        yield return new WaitUntil(() => currentAmountOfCustomersInShop < customerLines.Count * 3);

        if (gameManager.currentDayTimer < 1 || customerOrders.Count <= 0) yield break;
        
        currentAmountOfCustomersInShop++;
            
        var newCustomer = Instantiate(customerPrefab, customerSpawnPos.position, customerSpawnPos.rotation).GetComponent<Customer>();
        newCustomer.SetOrder(customerOrders[0]);
        customerOrders.Remove(customerOrders[0]);

        foreach (var line in customerLines)
        {
            Vector3 customerTargetPos;
            
            if (line.isOpen)
            {
                line.isOpen = false;
                customerTargetPos = customerLines[customerLines.IndexOf(line)].transform.position;
                newCustomer.SetTargetLine(customerTargetPos);
                newCustomer.GetComponent<NavMeshAgent>().SetDestination(customerTargetPos);
                break;
            }

            if(line == customerLines[customerLines.Count - 1])
            {
                customerTargetPos = customerLines[Random.Range(0, customerLines.Count)].transform.position;
                newCustomer.SetTargetLine(customerTargetPos);
                newCustomer.GetComponent<NavMeshAgent>().SetDestination(customerTargetPos);
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
        //currentDayCustomerText.text = currentDayCustomerServed + "/" + currentDayNumOfCustomers;
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