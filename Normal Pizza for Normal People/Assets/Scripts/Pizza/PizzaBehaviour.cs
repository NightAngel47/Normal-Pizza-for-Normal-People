using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehaviour : MonoBehaviour
{
    private List<PizzaIngredient> ingredientsOnPizza = new List<PizzaIngredient>();

    //Oven/Cooking Variables
    [HideInInspector]
    public float cookedTime = 0; //the amount of time the pizza has spent in the oven
    public float counterTime = -1;
    public bool isBurnt = false; //has the pizza been burnt
    public bool isCooked = false; //has the pizza been cooked ----- only one of these two bools will be on at any given time
    public bool overCooking = false; //is it being over cooked, has it been cooked and will it be burning soon

    private MeshRenderer pizzaModelMat;
    public Material cooked;
    public Material burnt;
    private Material raw;

    public void Start()
    {
        pizzaModelMat = gameObject.GetComponentInChildren<MeshRenderer>();
        raw = pizzaModelMat.material;
    }

    public void ResetPizzaOrientation()
    {
        transform.GetChild(0).Rotate(0, 0, 0);
    }

    public void AddPizzaIngredient(PizzaIngredient newIngredient)
    {
        ingredientsOnPizza.Add(newIngredient);
    }

    public List<PizzaIngredient> GetIngredientsOnPizza()
    {
        return ingredientsOnPizza;
    }

    public void MaterialChangeBack()
    {
        if(isCooked)
        {
            pizzaModelMat.material = cooked;
        }

        if (isBurnt)
        {
            pizzaModelMat.material = burnt;
        }

        if(isCooked == false && isBurnt == false)
        {
            pizzaModelMat.material = raw;
        }
    }
}
