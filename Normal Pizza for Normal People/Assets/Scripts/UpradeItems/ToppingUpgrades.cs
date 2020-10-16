/*
 * Normal Pizza for Normal People
 * IM 389
 * ToppingUpgrades
 * Sydney & Steven
 * Sydney: Created initial script that handled adding new toppings to world and order creation
 * Steven: Added new topping UI
 * Sets up new topping basket when the upgraded has been given. Turns on object and reactivates it
 */

using System.Collections;
using UnityEngine;

public class ToppingUpgrades : ItemUpgrades
{
    public override void TurnOnUpgrade()
    {
        PizzaIngredient ingredient = GetComponent<PizzaIngredientSpawner>().pizzaIngredientToSpawn.GetComponent<IngredientHitEffect>().spawnObjectOnCollision.GetComponent<PizzaIngredient>();
        switch (ingredient.tier)
        {
            case 1:
                FindObjectOfType<OrderCreation>().tierOneIngredients.Add(ingredient);
                break;
            case 2:
                FindObjectOfType<OrderCreation>().tierTwoIngredients.Add(ingredient);
                break;
            case 3:
                FindObjectOfType<OrderCreation>().tierThreeIngredients.Add(ingredient);
                break;
            default:
                Debug.LogWarning($"Ingredient's tier value, {ingredient.tier} is not supported.");
                break;
        }
    }
}
