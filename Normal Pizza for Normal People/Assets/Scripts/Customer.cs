using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private Order order;
    private MoneyTracker moneyTracker;
    
    // Start is called before the first frame update
    void Start()
    {
        moneyTracker = FindObjectOfType<GameManager>().GetMoneyTracker();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out PizzaBehaviour pizza))
        {
            moneyTracker.ChangeMoney(CheckDeliveredPizza(pizza));
        }
    }

    /// <summary>
    /// Sets the customer's order when spawned.
    /// </summary>
    /// <param name="customerOrder">The order that the customer will have</param>
    public void SetOrder(Order customerOrder)
    {
        order = customerOrder;
    }

    /// <summary>
    /// Checks if the delivered pizza is valid. Returns money earned or lost based on if pizza was correct.
    /// </summary>
    /// <param name="pizza">The pizza being checked.</param>
    /// <returns>If true: it returns the amount of money the pizza earned, else: it returns the amount of money lost.</returns>
    private int CheckDeliveredPizza(PizzaBehaviour pizza)
    {
        return pizza.GetIngredientsOnPizza() == order.GetOrderIngredients() ? 100 : 0;
    }
}
