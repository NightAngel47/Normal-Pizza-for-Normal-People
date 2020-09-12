/*
 * Normal Pizza for Normal People
 * IM 491
 * CustomerUI
 * Steven
 * Steven: Handles customer ui, including order and timer display
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    [SerializeField] private GameObject orderTimerUI = null;
    private TMP_Text orderTimerText = null;
    private Image orderTimerProgressBar = null;
    private enum OrderTimerStates {Start, Middle, Quarter, End}
    [SerializeField] private List<Color> orderTimerColors = new List<Color>();
    
    [SerializeField] private Transform ingredientUITransform = null;
    [SerializeField] private GameObject ingredientUI = null;
    [SerializeField] private GameObject speechBubbleUI = null;

    [SerializeField] private GameObject moneyForOrderText = null;
    
    private Camera vrCam = null;

    private void Start()
    {
        vrCam = Camera.main;
        orderTimerText = orderTimerUI.GetComponentInChildren<TMP_Text>();
        orderTimerProgressBar = orderTimerUI.transform.GetChild(0).GetComponent<Image>();
        
        ToggleOrderUIState();
    }
    
    /// <summary>
    /// Toggles the customer order UI on/off
    /// </summary>
    public void ToggleOrderUIState()
    {
        ingredientUITransform.gameObject.SetActive(!ingredientUITransform.gameObject.activeSelf);
        orderTimerUI.SetActive(!orderTimerUI.activeSelf);
        speechBubbleUI.SetActive(!speechBubbleUI.activeSelf);
    }

    /// <summary>
    /// Creates the ui elements for the toppings from the customer's order
    /// </summary>
    /// <param name="uniqueIngredients">The unique toppings and their ammounts</param>
    public void CreateToppingUI(Dictionary<PizzaIngredient, int> uniqueIngredients)
    {
        foreach (var ingredient in uniqueIngredients)
        {
            // instantiate UI
            var newIngredient = Instantiate(ingredientUI, ingredientUITransform.position, ingredientUITransform.rotation, ingredientUITransform);
            
            // update text with info
            var ingredientTexts = newIngredient.GetComponentsInChildren<TMP_Text>();
            ingredientTexts[0].text = ingredient.Key.GetIngredientName();
            ingredientTexts[1].text = "x" + ingredient.Value;

            newIngredient.GetComponentInChildren<Image>().sprite = ingredient.Key.GetIngredientIcon();
        }
    }
    
    /// <summary>
    /// Updates the progress of the order timer.
    /// </summary>
    /// <param name="startOrderTime">The starting amount for the order timer.</param>
    /// <param name="currentOrderTime">the current amount for the order timer.</param>
    public void UpdateOrderTimer(ref float startOrderTime, ref float currentOrderTime)
    {
        orderTimerText.text = ""+(int)currentOrderTime;
        orderTimerProgressBar.fillAmount = currentOrderTime / startOrderTime;
        
        if (currentOrderTime > 0)
        {
            if (currentOrderTime / startOrderTime >= 0.66f)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.Start];
            }
            else if (currentOrderTime / startOrderTime <= .66f)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.Middle];
            }
            else if (currentOrderTime / startOrderTime <= .33f)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.Quarter];
            }
            else if (currentOrderTime <= 10)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.End];
            }
        }
    }
    
    /// <summary>
    /// Shows the amount of money a pizza earned/loss
    /// </summary>
    /// <param name="amount">The calculated amount</param>
    public void ShowMoneyAmount(ref int amount)
    {
        var position = gameObject.transform.position;
        GameObject moneyForOrderTextObject = Instantiate(moneyForOrderText, new Vector3(position.x, 1, position.z), Quaternion.identity);
        moneyForOrderTextObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "$" + amount;
        moneyForOrderTextObject.transform.LookAt(vrCam.transform);
        moneyForOrderTextObject.gameObject.SetActive(true);

        moneyForOrderTextObject.transform.localPosition = new Vector3(position.x, 1, position.z);
        DestoryOrderUI();
        Destroy(moneyForOrderTextObject, 3);
    }

    /// <summary>
    /// Destroys the order UI
    /// </summary>
    public void DestoryOrderUI()
    {
        Destroy(ingredientUITransform.transform.parent.gameObject);
    }
}
