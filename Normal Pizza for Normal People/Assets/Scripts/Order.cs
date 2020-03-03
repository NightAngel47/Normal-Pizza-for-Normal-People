using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private List<PizzaIngredient> orderIngredients = new List<PizzaIngredient>();

    //TODO add set order
    
    public List<PizzaIngredient> GetOrderIngredients()
    {
        return orderIngredients;
    }
}
