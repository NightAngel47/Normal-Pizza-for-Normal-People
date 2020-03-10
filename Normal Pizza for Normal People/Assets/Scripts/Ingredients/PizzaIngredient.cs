using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaIngredient : MonoBehaviour
{
    [SerializeField, Tooltip("The name of the ingredient")]
    private string ingredientName;

    [SerializeField]
    private Sprite ingredientIcon;

    public string GetIngredientName()
    {
        return ingredientName;
    }

    public Sprite GetIngredientIcon()
    {
        return ingredientIcon;
    }
}
