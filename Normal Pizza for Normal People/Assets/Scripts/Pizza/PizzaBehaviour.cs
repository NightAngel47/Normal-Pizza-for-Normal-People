/*
 * Normal Pizza for Normal People
 * IM 389
 * PizzaBehaviour
 * Sydney & Steven
 * Sydney: Added pizza cooking, and hover UI
 * Steven: Created initial script that holds ingredients on pizza, helped with hover UI
 * Handles ingredients on pizza and cooking state, and also hover UI
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PizzaBehaviour : MonoBehaviour
{
    private readonly List<PizzaIngredient> ingredientsOnPizza = new List<PizzaIngredient>();
    private readonly List<GameObject> ingredientUIList = new List<GameObject>();
    private readonly List<int> uniqueIngredientCounts = new List<int>();

    //Oven/Cooking Variables
    [HideInInspector] public float cookedTime = 0; //the amount of time the pizza has spent in the oven
    public float counterTime = -1;
    public bool isBurnt = false; //has the pizza been burnt
    public bool isCooked = false; //has the pizza been cooked ----- only one of these two bools will be on at any given time
    public bool overCooking = false; //is it being over cooked, has it been cooked and will it be burning soon

    private MeshRenderer pizzaModelMat = null;
    public Material cooked = null;
    public Material burnt = null;
    private Material raw = null;

    public bool inOven = false;

    [SerializeField] private Transform ingredientUITransform = null;
    [SerializeField] private GameObject ingredientUI = null;

    public void Start()
    {
        pizzaModelMat = gameObject.GetComponentInChildren<MeshRenderer>();
        raw = pizzaModelMat.material;
    }

    public void AddPizzaIngredient(PizzaIngredient newIngredient)
    {
        ingredientsOnPizza.Add(newIngredient);
        UpdateCanvas(newIngredient);
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

    private void UpdateCanvas(PizzaIngredient addedIngredient)
    {
        // checks if added ingredient already has UI
        foreach (var uiElement in ingredientUIList)
        {
            var ingredientTexts = uiElement.GetComponentsInChildren<TMP_Text>();
            if (addedIngredient.GetIngredientName() == ingredientTexts[0].text)
            {
                ++uniqueIngredientCounts[ingredientUIList.IndexOf(uiElement)];
                ingredientTexts[1].text = "x" + uniqueIngredientCounts[ingredientUIList.IndexOf(uiElement)];
                return;
            }
        }
        
        // if there is no UI create one
        CreateNewUI(addedIngredient);
    }

    private void CreateNewUI(PizzaIngredient addedIngredient)
    {

        // instantiate UI
        var newIngredient = Instantiate(ingredientUI, ingredientUITransform.position, ingredientUITransform.rotation, ingredientUITransform);
            
        // add new UI to list of UI for updating
        ingredientUIList.Add(newIngredient);
        uniqueIngredientCounts.Add(1);
            
        // update text with info
        var ingredientTexts = newIngredient.GetComponentsInChildren<TMP_Text>();
        ingredientTexts[0].text = addedIngredient.GetIngredientName();
        ingredientTexts[1].text = "x" + uniqueIngredientCounts[ingredientUIList.IndexOf(newIngredient)];

        newIngredient.GetComponentInChildren<Image>().sprite = addedIngredient.GetIngredientIcon();
    }

    public void CurrentIngredients()
    {
        //turn on canvas
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void TurnOffCurrentIngredients()
    {
        //turn off canvas
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SetZeroRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
