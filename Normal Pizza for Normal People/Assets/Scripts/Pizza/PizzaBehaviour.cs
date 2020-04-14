using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Transform ingredientUITransform;
    [SerializeField]
    private GameObject ingredientUI;

    public void Start()
    {
        pizzaModelMat = gameObject.GetComponentInChildren<MeshRenderer>();
        raw = pizzaModelMat.material;
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

    public void CurrentIngredients()
    {
        //turn on canvas
        gameObject.transform.GetChild(1).gameObject.SetActive(true);

        List<PizzaIngredient> uniqueIngredients = new List<PizzaIngredient>();
        foreach (var ingredient in ingredientsOnPizza.Where(ingredient => !uniqueIngredients.Contains(ingredient)))
        {
            uniqueIngredients.Add(ingredient);
        }

        // for each unique order ingredient
        foreach (var ingredient in uniqueIngredients)
        {
            // get unique ingredient count
            int uniqueIngredientCount = ingredientsOnPizza.Count(orderIngredient => ingredient == orderIngredient);

            // instantiate UI
            var newIngredient = Instantiate(ingredientUI, ingredientUITransform.position, ingredientUITransform.rotation, ingredientUITransform);

            // update text with info
            var ingredientTexts = newIngredient.GetComponentsInChildren<TMP_Text>();
            ingredientTexts[0].text = ingredient.GetIngredientName();
            ingredientTexts[1].text = "x" + uniqueIngredientCount;

            newIngredient.GetComponentInChildren<Image>().sprite = ingredient.GetIngredientIcon();
        }
    }

    public void TurnOffCurrentIngredients()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
