using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private Order order;
    private GameManager gm;
    private MoneyTracker moneyTracker;
    private CustomerLine customerLine;
    private NavMeshAgent agent;
    
    [SerializeField]
    private float startOrderTime = 90f;
    private float currentOrderTime;
    [SerializeField]
    private GameObject orderTimerUI;
    private TMP_Text orderTimerText;
    private Image orderTimerProgressBar;
    
    [SerializeField]
    private Transform ingredientUITransform;
    [SerializeField]
    private GameObject ingredientUI;

    public bool activeOrder;
    private bool leaving;
    private Vector3 targetLinePos;
    private Vector3 endPos;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        customerLine = gm.GetComponent<CustomerLine>();
        moneyTracker = gm.GetMoneyTracker();
        agent = GetComponent<NavMeshAgent>();
        endPos = transform.position + (Vector3.right * 14);
        
        orderTimerText = orderTimerUI.GetComponentInChildren<TMP_Text>();
        orderTimerProgressBar = orderTimerUI.transform.GetChild(0).GetComponent<Image>();
        currentOrderTime = startOrderTime + 1;
        ChangeUIState();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LineStart"))
        {
            activeOrder = true;
            ChangeUIState();
            StartCoroutine(OrderTimerCountDown());
        }
        
        if (other.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        {
            moneyTracker.ChangeMoney(CheckDeliveredPizza(pizza));
            Destroy(pizza.gameObject);
            activeOrder = false;
            CustomerLeave();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LineStart"))
        {
            activeOrder = false;
            ChangeUIState();
        }
    }

    /// <summary>
    /// Sets the customer's order when spawned.
    /// </summary>
    /// <param name="customerOrder">The order that the customer will have</param>
    public void SetOrder(Order customerOrder)
    {
        order = customerOrder;
        DisplayOrder();
    }

    /// <summary>
    /// Calculates the number of unique ingredients.
    /// Instantiates ingredient ui for each unique ingredient with ingredient count.
    /// </summary>
    private void DisplayOrder()
    {
        //TODO decouple UI from customer
        // get unique ingredients
        List<PizzaIngredient> uniqueIngredients = new List<PizzaIngredient>();
        foreach (var ingredient in order.GetOrderIngredients().Where(ingredient => !uniqueIngredients.Contains(ingredient)))
        {
            uniqueIngredients.Add(ingredient);
        }

        // for each unique order ingredient
        foreach (var ingredient in uniqueIngredients)
        {
            // get unique ingredient count
            int uniqueIngredientCount = order.GetOrderIngredients().Count(orderIngredient => ingredient == orderIngredient);

            // instantiate UI
            var newIngredient = Instantiate(ingredientUI, ingredientUITransform.position, ingredientUITransform.rotation, ingredientUITransform);
            
            // update text with info
            var ingredientTexts = newIngredient.GetComponentsInChildren<TMP_Text>();
            ingredientTexts[0].text = ingredient.GetIngredientName();
            ingredientTexts[1].text = "x" + uniqueIngredientCount;

            newIngredient.GetComponentInChildren<Image>().sprite = ingredient.GetIngredientIcon();
        }
    }

    private IEnumerator OrderTimerCountDown()
    {
        orderTimerText.text = ""+(int)currentOrderTime;
        orderTimerProgressBar.fillAmount = currentOrderTime / startOrderTime;
        yield return new WaitForEndOfFrame();
        currentOrderTime -= Time.deltaTime;
        if (currentOrderTime > 0)
        {
            StartCoroutine(OrderTimerCountDown());
        }
        else
        {
            moneyTracker.ChangeMoney(-100);
            activeOrder = false;
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
        //TODO add pizza money calculation
        
        if (pizza.GetIngredientsOnPizza().Count != order.GetOrderIngredients().Count) return -100;

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
        
        if (tempOrderList.Count > 0) return -100;

        if (gm.currentDay > 0)
        {
            if (pizza.isBurnt || pizza.isCooked == false)
            {
                return -50;
            }
        }
        
        return 100;
    }

    public void CustomerLeave()
    {
        if (leaving || activeOrder) return;
        
        agent.SetDestination(endPos);
        leaving = true;
        Invoke(nameof(CallTheNextCustomer), 5f);
        Destroy(gameObject, 10f);
    }

    private void CallTheNextCustomer()
    {
        customerLine.CustomerServed();
    }

    private void ChangeUIState()
    {
        ingredientUITransform.gameObject.SetActive(!ingredientUITransform.gameObject.activeSelf);
        orderTimerUI.SetActive(!orderTimerUI.activeSelf);
    }
}
