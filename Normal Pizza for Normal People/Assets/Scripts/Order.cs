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
