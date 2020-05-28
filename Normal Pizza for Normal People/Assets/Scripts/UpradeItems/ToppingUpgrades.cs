﻿/*
 * Normal Pizza for Normal People
 * IM 389
 * ToppingUpgrades
 * Sydney & Steven
 * Sydney: Created initial script that handled adding new toppings to world and order creation
 * Steven: Added new topping UI
 * Sets up new topping basket when the upgraded has been given. Turns on object and reactivates it
 */

using System.Collections;
using UnityEngine;

public class ToppingUpgrades : ItemUpgrades
{
    [SerializeField] private GameObject newToppingUI = null;

    private GameObject toppingUIInstance = null;
    
    private PizzaIngredientSpawner pis = null;

    public override void TurnOnUpgrade()
    {
        pis = gameObject.GetComponent<PizzaIngredientSpawner>();
        pis.enabled = !pis.enabled;

        if (pis.enabled)
        {
            FindObjectOfType<OrderCreation>().allPizzaIngredients.Add(pis.pizzaIngredientToSpawn.GetComponent<IngredientHitEffect>().spawnObjectOnCollision.GetComponent<PizzaIngredient>());
            toppingUIInstance = Instantiate(newToppingUI, transform.position, Quaternion.Euler(0, -90, 0));
            StartCoroutine(DestroyUI());
        }
    }

    private IEnumerator DestroyUI()
    {
        yield return new WaitUntil(() => FindObjectOfType<GameManager>().dayStarted);
        Destroy(toppingUIInstance);
    }
}
