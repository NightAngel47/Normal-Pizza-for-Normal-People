using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToppingOrderUI : MonoBehaviour
{
    [SerializeField, Tooltip("UI Image for Icon")]
    private Image iconImg;
    [SerializeField, Tooltip("UI Text for Label")]
    private TMP_Text labelText;
    [SerializeField, Tooltip("UI Text for Amount")]
    private TMP_Text amountText;

    /// <summary>
    /// Sets up topping UI with for a single topping 
    /// </summary>
    /// <param name="icon">Sprite icon image for the topping</param>
    /// <param name="label">String name of the topping</param>
    /// <param name="amount">Int amount of the topping</param>
    public void SetUI(Sprite icon, string label, int amount)
    {
        iconImg.sprite = icon;
        labelText.text = label;
        amountText.text = "x" + amount;
    }

    /// <summary>
    /// Updates topping amount UI
    /// </summary>
    /// <param name="amount">The new amount for that topping</param>
    public void UpdateAmount(int amount)
    {
        amountText.text = "x" + amount;
    }
    
    /// <summary>
    /// Gets the name of the topping for this topping order UI
    /// </summary>
    /// <returns>The name of the topping for this topping order UI</returns>
    public string ToppingName()
    {
        return labelText.text;
    }
}
