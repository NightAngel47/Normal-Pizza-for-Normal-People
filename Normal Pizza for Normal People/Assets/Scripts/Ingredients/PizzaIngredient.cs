/*
 * Normal Pizza for Normal People
 * IM 389
 * PizzaIngredientSpawner
 * Steven & Sydney
 * Steven: Created initial script that holds ingredient info
 * Sydney: Added ingredient icon info
 * Handles ingredient info and icon
 */

using UnityEngine;

public class PizzaIngredient : MonoBehaviour
{
    [SerializeField, Tooltip("The name of the ingredient")] private string ingredientName = null;

    [SerializeField] private Sprite ingredientIcon = null;

    public int tier = 0;
    public bool isCheese;
    public bool isDough;

    public string GetIngredientName()
    {
        return ingredientName;
    }

    public Sprite GetIngredientIcon()
    {
        return ingredientIcon;
    }

    public int GetIngredientTier()
    {
        return tier;
    }
}
