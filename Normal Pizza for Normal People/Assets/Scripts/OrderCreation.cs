using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderCreation : MonoBehaviour
{
    [SerializeField]
    public List<PizzaIngredient> allPizzaIngredients = new List<PizzaIngredient>();
    [SerializeField]
    private int minIngredientsPerOrder = 1;
    [SerializeField]
    private int maxIngredientsPerOrder = 3;

    /// <summary>
    /// Generates the orders for the number of customers
    /// </summary>
    /// <param name="numOfCustomers">The number of orders to generates</param>
    /// <returns>Returns list of orders for customers</returns>
    public List<Order> GenerateOrders(int numOfCustomers)
    {
        List<Order> orders = new List<Order>(numOfCustomers);
        
        for (int i = 0; i < orders.Capacity; ++i)
        {
            orders.Add(new Order(RandomOrderIngredients()));
        }
        
        return orders;
    }

    /// <summary>
    /// Generates random ingredients from list of all ingredients. Uses the ingredients per order for number of ingredients to generate. 
    /// </summary>
    /// <returns>Returns list of ingredients for order for the number of ingredients per order</returns>
    private List<PizzaIngredient> RandomOrderIngredients()
    {
        int randNumOfIngredients = Random.Range(minIngredientsPerOrder, maxIngredientsPerOrder + 1);
        List<PizzaIngredient> ingredients = new List<PizzaIngredient>(randNumOfIngredients);
        
        for (int i = 0; i < ingredients.Capacity; ++i)
        {
            ingredients.Add(allPizzaIngredients[Random.Range(0, allPizzaIngredients.Count)]);
        }

        return ingredients;
    }
}
