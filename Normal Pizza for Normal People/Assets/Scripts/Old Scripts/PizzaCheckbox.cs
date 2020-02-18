using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCheckbox : MonoBehaviour
{
    [SerializeField, Tooltip("The matching current pizza status UI gameobject")]
    private CurrentPizzaStatus currentPizzaStatusUI;
    /// <summary>
    /// List of the current topping names on the pizza
    /// </summary>
    private List<string> currentToppingNames;
    /// <summary>
    /// List of the current topping amounts on the pizza
    /// </summary>
    private List<int> currentToppingAmounts;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("pizza"))
        {
            //TODO reset UI then update UI with current pizza status
            PizzaStatus(other.transform);
            currentPizzaStatusUI.SetupUI(currentToppingNames, currentToppingAmounts);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("pizza"))
        {
            //TODO update UI with current pizza status
            PizzaStatus(other.transform);
            currentPizzaStatusUI.UpdateUI(currentToppingNames, currentToppingAmounts);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("pizza"))
        {
            //TODO reset UI
            currentPizzaStatusUI.ResetUI();
        }
    }

    private void PizzaStatus(Transform other)
    {
        // for all topping children
        for (int i = 0; i < other.childCount; ++i)
        {
            // if first
            if (i == 0)
            {
                currentToppingNames.Add(other.name);
                currentToppingAmounts.Add(1);
            }
            else // rest of toppings
            {
                // check list of current toppings
                for (int j = 0; j < currentToppingNames.Count; ++j)
                {
                    if (other.name.Contains(currentToppingNames[j]))
                    {
                        currentToppingAmounts[j]++;
                    }
                }
            }
        }
    }
}
