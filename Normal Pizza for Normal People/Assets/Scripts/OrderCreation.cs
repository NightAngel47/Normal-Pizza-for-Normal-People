using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCreation : MonoBehaviour
{
    [SerializeField]
    private List<PizzaIngredient> allPizzaIngredients = new List<PizzaIngredient>();
    [SerializeField]
    private int ingredientsPerOrder = 3;
    
    /// <summary>
    /// Generates the orders for the number of customers
    /// </summary>
    /// <param name="numOfCustomers">The number of orders to generates</param>
    /// <returns>Returns list of orders for customers</returns>
    public List<Order> GenerateOrders(int numOfCustomers)
    {
        List<Order> orders = new List<Order>(numOfCustomers);
        
        for (int i = 0; i < orders.Count; ++i)
        {
            Order newOrder = new Order(RandomOrderIngredients());
            orders.Add(newOrder);
        }
        
        return orders;
    }

    /// <summary>
    /// Generates random ingredients from list of all ingredients. Uses the ingredients per order for number of ingredients to generate. 
    /// </summary>
    /// <returns>Returns list of ingredients for order for the number of ingredients per order</returns>
    private List<PizzaIngredient> RandomOrderIngredients()
    {
        List<PizzaIngredient> ingredients = new List<PizzaIngredient>(ingredientsPerOrder);
        for (int i = 0; i < ingredients.Count; ++i)
        {
            ingredients.Add(allPizzaIngredients[Random.Range(0, allPizzaIngredients.Count)]);
        }

        return ingredients;
    }
}
