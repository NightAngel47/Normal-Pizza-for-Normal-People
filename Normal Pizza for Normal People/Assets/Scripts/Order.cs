using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private readonly List<PizzaIngredient> orderIngredients;

    public Order(List<PizzaIngredient> pizzaIngredients)
    {
        orderIngredients = pizzaIngredients;
    }

    public List<PizzaIngredient> GetOrderIngredients()
    {
        return orderIngredients;
    }
}
