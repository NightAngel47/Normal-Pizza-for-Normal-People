/*
 * Normal Pizza for Normal People
 * IM 389 & IM 491
 * Customer
 * Steven & Sydney
 * Steven: Created initial script that handles customer AI, order and timer, and delivery of pizza
 *     Refactored customer to be split into Customer, CustomerAI, CustomerUI, and CustomerAudio
 * Sydney: The show money amount function and the variables it sets and uses
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customer : MonoBehaviour
{
    // Customer Components
    public CustomerAudio customerAudio = null;
    public CustomerUI customerUI = null;
    public CustomerAI customerAI = null;
    
    private Order order = null;
    
    private GameManager gm = null;
    private MoneyTracker moneyTracker = null;
    
    private CustomerLine customerLine = null;
    
    // Customer Order Variables
    [SerializeField] private float startOrderTime = 90f;
    private float currentOrderTime = 0;
    public bool activeOrder { get; private set; }
    
    // Tutorial flag
    public static bool firstPizzaThrow = false;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        customerLine = gm.GetComponent<CustomerLine>();
        moneyTracker = gm.GetMoneyTracker();

        customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Walking);
        
        currentOrderTime = startOrderTime + 1;
    }

    /// <summary>
    /// Sets the line that the customer will initially head towards.
    /// </summary>
    /// <param name="customerLinePos">The position of the line.</param>
    public void SetTargetLine(Vector3 customerLinePos)
    {
        customerAI.SetTargetLine(customerLinePos);
    }

    private void Update()
    {
        // play customer walk sound as long as the customer is not stopped
        if (customerAI.currentCustomerAIState != CustomerAI.CustomerAIStates.Stopped && customerAudio.CurrentCustomerAudioState != CustomerAudio.CustomerAudioStates.Walking)
        {
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Walking);
        }
        // stop customer walk sound when customer stopped
        else if (customerAudio.CurrentCustomerAudioState == CustomerAudio.CustomerAudioStates.Walking && customerAI.currentCustomerAIState == CustomerAI.CustomerAIStates.Stopped)
        {
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Stop);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // when the customer reaches the front of the line
        if (other.CompareTag("LineStart"))
        {
            activeOrder = true;
            transform.rotation = other.transform.rotation;
            customerAI.ChangeCustomerAIState(CustomerAI.CustomerAIStates.Stopped);
            customerUI.ToggleOrderUIState();
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.AtCounter);
            StartCoroutine(OrderTimerCountDown());
        }
        
        // when the customer is delivered a pizza
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
        // while the customer is at the front of the line and not facing the right direction
        if (other.CompareTag("LineStart") && transform.rotation == other.transform.rotation)
        {
            transform.rotation = other.transform.rotation;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // when the customer leaves the front of the line
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
        
        customerUI.CreateToppingUI(uniqueIngredients);
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
        // adds base profit for pizza
        float deliveredPizzaMoney = moneyTracker.basePizzaProfit;

        // adds profit for toppings based on tiers
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

        // checks flag for the first pizza thrown for the tutorial flag
        firstPizzaThrow = true;

        // checks if the ingredients on the pizza match the customer's order
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
                    return (int) -deliveredPizzaMoney / 2; // bad order because pizza is uncooked or burnt
                }
                
                // add cooked bonus
                if (pizza.isCooked)
                {
                    deliveredPizzaMoney += (int) moneyTracker.cookedBonus;
                }
                //TODO add other bonuses
                
                customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.GoodOrder);
                return (int) (deliveredPizzaMoney * (1 + currentOrderTime / startOrderTime)); // good order because pizza is correct
            }
        }

        customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.BadOrder);
        return (int) -deliveredPizzaMoney / 2; // bad order because the total amount of toppings on pizza is not equal to customer order
    }

    /// <summary>
    /// Tells the customer to leave
    /// </summary>
    public void CustomerLeave()
    {
        if (customerAI.currentCustomerAIState == CustomerAI.CustomerAIStates.Leaving || activeOrder) return;
        
        customerAI.Leave();
        
        customerLine.IncreaseCustomersServed();
        Invoke(nameof(CallTheNextCustomer), 5f);
        Destroy(gameObject, 10f);
    }

    /// <summary>
    /// Tells the customer line to bring in the next customer
    /// </summary>
    private void CallTheNextCustomer()
    {
        customerLine.CustomerServed();
    }
}