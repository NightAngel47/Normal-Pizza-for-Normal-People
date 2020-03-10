using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private Order order;
    private MoneyTracker moneyTracker;

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

    private GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

        moneyTracker = FindObjectOfType<GameManager>().GetMoneyTracker();

        orderTimerText = orderTimerUI.GetComponentInChildren<TMP_Text>();
        orderTimerProgressBar = orderTimerUI.transform.GetChild(0).GetComponent<Image>();
        currentOrderTime = startOrderTime + 1;
        StartCoroutine(OrderTimerCountDown());
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.transform.parent.TryGetComponent(out PizzaBehaviour pizza)) return;
        moneyTracker.ChangeMoney(CheckDeliveredPizza(pizza));
        Destroy(pizza.gameObject);
        Destroy(gameObject);
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
            var newIngredient = Instantiate(ingredientUI, ingredientUITransform.position, Quaternion.identity, ingredientUITransform);
            
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
            Destroy(gameObject);
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

        if (gm.currentDay >= 0)
        {
            if (pizza.isBurnt == true || pizza.isCooked == false)
            {
                return -50;
            }
        }
        
        return 100;
    }
}
