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
        
        ChangeUIState();
    }
    
    public void ChangeUIState()
    {
        ingredientUITransform.gameObject.SetActive(!ingredientUITransform.gameObject.activeSelf);
        orderTimerUI.SetActive(!orderTimerUI.activeSelf);
        speechBubbleUI.SetActive(!speechBubbleUI.activeSelf);
    }

    public void DisplayOrder(ref Dictionary<PizzaIngredient, int> uniqueIngredients)
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

    public void DestoryOrderUI()
    {
        Destroy(ingredientUITransform.transform.parent.gameObject);
    }
}
