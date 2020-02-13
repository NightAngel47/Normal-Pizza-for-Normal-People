using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerTest : MonoBehaviour
{
    private List<GameObject> orderIngredients = new List<GameObject>();

    public GameObject testTopping;

    public void Start()
    {
        orderIngredients.Add(testTopping);
    }

    private void OnTriggerEnter(Collider col)
    {
        // is ths collider a pizza
        if (!col.TryGetComponent(out PizzaScript pizza)) return;

        if (IsOrderCorrect(pizza))
        {
            //TODO order and pizza match
        }
        else
        {
            //TODO order and pizza don't match
        }
    }

    /// <summary>
    /// Checks if ingredients on pizza are the same as ingredients in order
    /// </summary>
    /// <param name="pizza">The pizza being checked.</param>
    /// <returns>Returns true if pizza and order match. Returns false if they don't.</returns>
    private bool IsOrderCorrect(PizzaScript pizza)
    {
        List<GameObject> tempPizzaIngredients = pizza.ingredientList;
        List<GameObject> tempOrderIngredients = orderIngredients;
        
        foreach (var orderIngredient in tempOrderIngredients)
        {
            foreach (var pizzaIngredient in tempPizzaIngredients.Where(pizzaIngredient => orderIngredient.name == pizzaIngredient.name))
            {
                tempOrderIngredients.Remove(orderIngredient);
                tempPizzaIngredients.Remove(pizzaIngredient);
            }
        }

        return tempOrderIngredients.Count <= 0 && tempPizzaIngredients.Count <= 0;
    }
}
