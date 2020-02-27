using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old_Scripts
{
    public class CurrentPizzaStatus : MonoBehaviour
    {
        private GameManager gm;
        [SerializeField, Tooltip("Gameobject of topping UI for spawning dynamically")]
        private GameObject toppingUIPrefab;
        /// <summary>
        /// List of topping UI elements
        /// </summary>
        private List<ToppingOrderUI> toppingUI = new List<ToppingOrderUI>();
        [SerializeField, Tooltip("The transform to parent the new UI to")]
        private Transform uiTransform;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            ResetUI();
        }

        public void SetupUI(List<string> toppingNames, List<int> toppingCount)
        {
            ResetUI();
        
            for (int i = 0; i < toppingNames.Count; ++i)
            {
                GameObject newToppingUI = Instantiate(toppingUIPrefab, uiTransform);

                int iconIndex;
                for (iconIndex = 0; iconIndex < gm.toppingNames.Length; ++iconIndex)
                {
                    if (toppingNames[i] != gm.toppingNames[iconIndex]) continue;
                    ToppingOrderUI newToppingOrderUI = newToppingUI.GetComponent<ToppingOrderUI>();
                    newToppingOrderUI.SetUI(gm.toppingIcons[iconIndex], toppingNames[i], toppingCount[i]);
                    toppingUI.Add(newToppingOrderUI);
                
                    break;
                }
            }
        }

        public void UpdateUI(List<string> toppingNames, List<int> toppingCount)
        {
            foreach (var topping in toppingUI)
            {
                for (int i = 0; i < toppingNames.Count; ++i)
                {
                    if (topping.ToppingName() == toppingNames[i])
                    {
                        topping.UpdateAmount(toppingCount[i]);
                    }
                }
            }
        }

        public void ResetUI()
        {
            foreach (var currentToppingUI in toppingUI)
            {
                Destroy(currentToppingUI.gameObject);
            }
        }
    }
}


