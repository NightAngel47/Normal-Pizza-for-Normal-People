﻿/*
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

    enum FaceTypes
    {
        Neutral,
        Good,
        Bad,
        Waiting,
        Speaking
    }
    public List<Material> faces = new List<Material>(); //neutral, good, bad, waiting, giving order
    private MeshRenderer head;
    
    private Order order = null;
    
    private GameManager gm = null;
    private MoneyTracker moneyTracker = null;
    
    private CustomerLine customerLine = null;
    
    // Customer Order Variables
    [SerializeField] private float startOrderTime = 90f;
    private float currentOrderTime = 0;
    public bool activeOrder { get; private set; }
    private bool wasGivenPizza = false;

    public float customerLeaveDelayTime = 1f;
    
    // Tutorial flag
    public static bool firstPizzaThrow = false;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        customerLine = gm.GetComponent<CustomerLine>();
        moneyTracker = gm.GetMoneyTracker();

        customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Walking);
        
        currentOrderTime = startOrderTime + 1;

        head = gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Sets the line that the customer will initially head towards.
    /// </summary>
    /// <param name="customerLinePos">The CustomerLinePos that they player will head towards.</param>
    public void SetTargetLine(CustomerLinePos customerLinePos)
    {
        customerAI.SetTargetLine(customerLinePos);
        customerAI.ChangeCustomerAIState(CustomerAI.CustomerAIStates.Entering);
    }

    private void OnTriggerEnter(Collider other)
    {
        // when the customer reaches the front of the line
        if (other.CompareTag("LineStart"))
        {
            activeOrder = true;
            transform.rotation = other.transform.rotation;
            customerAI.ChangeCustomerAIState(CustomerAI.CustomerAIStates.AtCounter);
            customerUI.ToggleOrderUIState();
            head.material = faces[(int) FaceTypes.Speaking];
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.AtCounter);
            StartCoroutine(OrderTimerCountDown());
        }
        
        // when the customer is delivered a pizza
        if (!wasGivenPizza && other.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        {
            wasGivenPizza = true;
            activeOrder = false;
            
            int pizzaProfit = CheckDeliveredPizza(pizza);

            if(pizzaProfit <= 0)
            {
                head.material = faces[(int) FaceTypes.Bad];
                customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.BadOrder);
            }

            if(pizzaProfit > 0)
            {
                head.material = faces[(int) FaceTypes.Good];
                customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.GoodOrder);
            }

            moneyTracker.CustomerChangeMoney(pizzaProfit);
            customerUI.ShowMoneyAmount(ref pizzaProfit);
            Destroy(pizza.gameObject);
            
            gm.RemoveActiveCustomer(this);
            StartCoroutine(CustomerLeave());
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
            if(startOrderTime - currentOrderTime >= 5 && startOrderTime - currentOrderTime < 15)
            {
                head.material = faces[(int) FaceTypes.Neutral];
            }

            if(startOrderTime - currentOrderTime >= 15)
            {
                head.material = faces[(int) FaceTypes.Waiting];
            }

            if (currentOrderTime <= 10)
            {
                customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.OrderEndingSoon);
            }
            
            StartCoroutine(OrderTimerCountDown());
        }
        else // order ran out of time and customer left
        {
            head.material = faces[(int) FaceTypes.Bad];
            moneyTracker.CustomerChangeMoney((int) -startOrderTime);
            customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.BadOrder);
            activeOrder = false;
            gm.RemoveActiveCustomer(this);
            customerUI.DestoryOrderUI();
            StartCoroutine(CustomerLeave());
        }
    }

    /// <summary>
    /// Checks if the delivered pizza is valid. Returns money earned or lost based on if pizza was correct.
    /// </summary>
    /// <param name="pizza">The pizza being checked.</param>
    /// <returns>If true: it returns the amount of money the pizza earned, else: it returns the amount of money lost.</returns>
    private int CheckDeliveredPizza(PizzaBehaviour pizza)
    {
        // checks flag for the first pizza thrown for the tutorial flag
        firstPizzaThrow = true;
        
        // adds base profit for pizza
        float deliveredPizzaMoney = moneyTracker.basePizzaProfit;
        int orderIngredientCount = 0;

        // adds profit for toppings based on tiers
        foreach (var ingredient in order.GetOrderIngredients().TakeWhile(ingredient => !ingredient.isCheese || !ingredient.isDough))
        {
            orderIngredientCount++;
            
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

        int pizzaIngredientCount = pizza.GetIngredientsOnPizza().TakeWhile(ingredient => !ingredient.isCheese || !ingredient.isDough).Count();

        // checks if the ingredients on the pizza match the customer's order
        if (pizzaIngredientCount == orderIngredientCount)
        {
            var tempOrderList = order.GetOrderIngredients();

            foreach (PizzaIngredient ingredient in pizza.GetIngredientsOnPizza())
            {
                if (!ingredient.isCheese && !ingredient.isDough)
                {
                    for (int j = 0; j < tempOrderList.Count; ++j)
                    {
                        if (ingredient.GetIngredientName() == tempOrderList[j].GetIngredientName())
                        {
                            tempOrderList.RemoveAt(j);
                            break;
                        }
                    }
                }
                else if (ingredient.isCheese)
                {
                    for (int j = 0; j < tempOrderList.Count; ++j)
                    {
                        if (ingredient.GetIngredientName() == tempOrderList[j].GetIngredientName())
                        {
                            deliveredPizzaMoney += moneyTracker.cheeseBonus;
                    
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
                            
                            tempOrderList.RemoveAt(j);
                            break;
                        }
                    }
                }
            }

            if (tempOrderList[0].isDough)
            {
                switch (tempOrderList[0].GetIngredientName())
                {
                    case "Uncooked":
                        if (!pizza.isCooked && !pizza.isBurnt)
                        {
                            deliveredPizzaMoney += moneyTracker.doughBonus;
                            tempOrderList.RemoveAt(0);
                        }
                        break;
                    case "Cooked":
                        if (pizza.isCooked && !pizza.isBurnt)
                        {
                            deliveredPizzaMoney += moneyTracker.doughBonus;
                            tempOrderList.RemoveAt(0);
                        }
                        break;
                    case "Burnt":
                        if (pizza.isBurnt)
                        {
                            deliveredPizzaMoney += moneyTracker.doughBonus;
                            tempOrderList.RemoveAt(0);
                        }
                        break;
                    default:
                        Debug.LogError("Missing Dough Type");
                        break;
                }
            }

            
            if (tempOrderList.Any(ingredient => !ingredient.isCheese && !ingredient.isDough))
            {
                return (int) -deliveredPizzaMoney / 2; // bad order because the toppings don't match
            }
                
            return (int) (deliveredPizzaMoney * (1 + currentOrderTime / startOrderTime)); // good order because pizza is correct
        }

        return (int) -deliveredPizzaMoney / 2; // bad order because the total amount of toppings on pizza is not equal to customer order
    }

    /// <summary>
    /// Tells the customer to leave. Waits a short period before walking away.
    /// </summary>
    public IEnumerator CustomerLeave()
    {
        if (customerAI.CurrentCustomerAIState == CustomerAI.CustomerAIStates.Leaving || activeOrder) yield break;
        
        customerAI.ChangeCustomerAIState(CustomerAI.CustomerAIStates.Leaving);
        
        yield return new WaitForSeconds(customerLeaveDelayTime);
        
        customerAI.Leave();
        customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Walking);
        
        customerLine.CustomerServed(this);
        Destroy(gameObject, 10f);
    }
}