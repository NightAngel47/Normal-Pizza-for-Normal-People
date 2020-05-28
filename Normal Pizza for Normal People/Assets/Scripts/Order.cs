/*
 * Normal Pizza for Normal People
 * IM 389
 * Order
 * Steven:
 * Holds order ingredients
 */

using System.Collections.Generic;

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
