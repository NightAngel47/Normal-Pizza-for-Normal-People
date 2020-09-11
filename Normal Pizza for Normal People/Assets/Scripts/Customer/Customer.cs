/*
 * Normal Pizza for Normal People
 * IM 389
 * Customer
 * Steven & Sydney
 * Steven: Created initial script that handles customer AI, order and timer, and delivery of pizza
 * Sydney: The show money amount function and the variables it sets and uses
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private Order order = null;
    private GameManager gm = null;
    private MoneyTracker moneyTracker = null;
    private CustomerLine customerLine = null;
    private NavMeshAgent agent = null;
    
    private CustomerAudio customerAudio = null;
    private CustomerUI customerUI = null;
    
    [SerializeField] private float startOrderTime = 90f;
    private float currentOrderTime = 0;

    [HideInInspector] public bool activeOrder = false;
    private bool leaving = false;
    private Vector3 targetLinePos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;

    public static bool firstPizzaThrow = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        customerLine = gm.GetComponent<CustomerLine>();
        moneyTracker = gm.GetMoneyTracker();
        agent = GetComponent<NavMeshAgent>();
        
        customerAudio = GetComponent<CustomerAudio>();
        customerUI = GetComponent<CustomerUI>();

        customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Walking);
        customerUI.ChangeUIState();
        
        endPos = transform.position + (Vector3.right * 14);
        currentOrderTime = startOrderTime + 1;
    }

    public void SetTargetLine(Vector3 customerLinePos)
    {
        targetLinePos = customerLinePos;
    }

    private void Update()
    {
        if (!leaving)
        {
            if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, 0.5f))
            {
                if (hit.collider.TryGetComponent(out Customer customer) && !customer.leaving)
                {
                    agent.SetDestination(transform.position);
                }
            }
            else if(agent.destination != targetLinePos)
            {
                agent.SetDestination(targetLinePos);
            }
        }

        if (agent.velocity.magnitude > 0 && customerAudio.CurrentCustomerAudioState != CustomerAudio.CustomerAudioStates.Walking)
        {
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Walking);
        }
        else if (customerAudio.CurrentCustomerAudioState == CustomerAudio.CustomerAudioStates.Walking && agent.remainingDistance <= 0)
        {
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Stop);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LineStart"))
        {
            activeOrder = true;
            transform.rotation = other.transform.rotation;
            customerUI.ChangeUIState();
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.AtCounter);
            StartCoroutine(OrderTimerCountDown());
        }
        
        if (other.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        {
            int pizzaProfit = CheckDeliveredPizza(pizza);
            moneyTracker.CustomerChangeMoney(pizzaProfit);
            customerUI.ShowMoneyAmount(ref pizzaProfit);
            Destroy(pizza.gameObject);
            activeOrder = false;
            gm.RemoveActiveCustomer(this);
            CustomerLeave();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LineStart") && transform.rotation == other.transform.rotation)
        {
            transform.rotation = other.transform.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LineStart"))
        {
            activeOrder = false;
        }
    }

    /// <summary>
    /// Sets the customer's order when spawned.
    /// </summary>
    /// <param name="customerOrder">The order that the customer will have</param>
    public void SetOrder(Order customerOrder)
    {
        order = customerOrder;
        CreateOrder();
    }

    /// <summary>
    /// Calculates the number of unique ingredients.
    /// Instantiates ingredient ui for each unique ingredient with ingredient count.
    /// </summary>
    private void CreateOrder()
    {
        // get unique ingredients and their count
        Dictionary<PizzaIngredient, int> uniqueIngredients = new Dictionary<PizzaIngredient, int>();
        foreach (var ingredient in order.GetOrderIngredients().Where(ingredient => !uniqueIngredients.ContainsKey(ingredient)))
        {
            uniqueIngredients.Add(ingredient, order.GetOrderIngredients().Count(orderIngredient => ingredient == orderIngredient));
        }
        
        customerUI.DisplayOrder(ref uniqueIngredients);
    }

    /// <summary>
    /// Handles order count down timer.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OrderTimerCountDown()
    {
        if (!activeOrder) yield break;
        
        customerUI.UpdateOrderTimer(ref startOrderTime, ref currentOrderTime);
        
        yield return new WaitForEndOfFrame();
        
        currentOrderTime -= Time.deltaTime;
        if (currentOrderTime > 0)
        {
            if (currentOrderTime <= 10)
            {
                customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.OrderEndingSoon);
            }
            
            StartCoroutine(OrderTimerCountDown());
        }
        else // order ran out of time and customer left
        {
            moneyTracker.CustomerChangeMoney((int) -startOrderTime);
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.BadOrder);
            activeOrder = false;
            gm.RemoveActiveCustomer(this);
            customerUI.DestoryOrderUI();
            CustomerLeave();
        }
    }

    /// <summary>
    /// Checks if the delivered pizza is valid. Returns money earned or lost based on if pizza was correct.
    /// </summary>
    /// <param name="pizza">The pizza being checked.</param>
    /// <returns>If true: it returns the amount of money the pizza earned, else: it returns the amount of money lost.</returns>
    private int CheckDeliveredPizza(PizzaBehaviour pizza)
    {
        float deliveredPizzaMoney = moneyTracker.basePizzaProfit;

        foreach (PizzaIngredient ingredient in order.GetOrderIngredients())
        {
            switch (ingredient.GetIngredientTier())
            {
                case 1:
                    deliveredPizzaMoney += moneyTracker.tier1Toppings;
                    break;
                case 2:
                    deliveredPizzaMoney += moneyTracker.tier2Toppings;
                    break;
                case 3:
                    deliveredPizzaMoney += moneyTracker.tier3Toppings;
                    break;
                default:
                    Debug.LogWarning("Ingredient: " + ingredient.name + " does not have supported tier.");
                    break;
            }
        }

        firstPizzaThrow = true;

        if (pizza.GetIngredientsOnPizza().Count == order.GetOrderIngredients().Count)
        {
            var tempOrderList = order.GetOrderIngredients();

            for (int i = 0; i < pizza.GetIngredientsOnPizza().Count; ++i)
            {
                for (int j = 0; j < tempOrderList.Count; ++j)
                {
                    if (pizza.GetIngredientsOnPizza()[i].GetIngredientName() == tempOrderList[j].GetIngredientName())
                    {
                        tempOrderList.RemoveAt(j);
                        break;
                    }
                }
            }

            if (tempOrderList.Count <= 0)
            {
                // first 2 days don't need oven because there is no oven to cook pizzas till day 3
                if (gm.currentGameDay.dayNum > 2 && (pizza.isBurnt || !pizza.isCooked))
                {
                    customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.BadOrder);
                    return (int) -deliveredPizzaMoney / 2;
                }
                
                if (pizza.isCooked)
                {
                    deliveredPizzaMoney += (int) moneyTracker.cookedBonus;
                }
                //TODO add other bonuses
                
                customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.GoodOrder);
                return (int) (deliveredPizzaMoney * (1 + currentOrderTime / startOrderTime));
            }
        }

        customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.BadOrder);
        return (int) -deliveredPizzaMoney / 2;
    }

    public void CustomerLeave()
    {
        if (leaving || activeOrder) return;
        
        agent.SetDestination(endPos);
        leaving = true;
        customerLine.IncreaseCustomersServed();
        Invoke(nameof(CallTheNextCustomer), 5f);
        Destroy(gameObject, 10f);
    }

    private void CallTheNextCustomer()
    {
        customerLine.CustomerServed();
    }
}