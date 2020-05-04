using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaIngredient : MonoBehaviour
{
    [SerializeField, Tooltip("The name of the ingredient")] private string ingredientName = null;

    [SerializeField] private Sprite ingredientIcon = null;

    public string GetIngredientName()
    {
        return ingredientName;
    }

    public Sprite GetIngredientIcon()
    {
        return ingredientIcon;
    }
}
