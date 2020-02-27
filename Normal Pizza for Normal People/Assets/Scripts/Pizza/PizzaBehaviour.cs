using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehaviour : MonoBehaviour
{
    private List<PizzaIngredient> ingredientsOnPizza = new List<PizzaIngredient>();

    //Oven/Cooking Variables
    public float cookedTime = 0; //the amount of time the pizza has spent in the oven
    public bool isBurnt = false; //has the pizza been burnt
    public bool isCooked = false; //has the pizza been cooked ----- only one of these two bools will be on at any given time
    public bool overCooking = false; //is it being over cooked, has it been cooked and will it be burning soon

    public void AddPizzaIngredient(PizzaIngredient newIngredient)
    {
        ingredientsOnPizza.Add(newIngredient);
    }

    public List<PizzaIngredient> GetIngredientsOnPizza()
    {
        return ingredientsOnPizza;
    }
}
