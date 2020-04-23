using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingUpgrades : ItemUpgrades
{
    [SerializeField] private GameObject newToppingUI;
    [SerializeField] private float newToppingUILifetime = 2f;
    
    private PizzaIngredientSpawner pis;

    public override void TurnOnUpgrade()
    {
        pis = gameObject.GetComponent<PizzaIngredientSpawner>();
        pis.enabled = !pis.enabled;

        if (pis.enabled)
        {
            FindObjectOfType<OrderCreation>().allPizzaIngredients.Add(gameObject.GetComponent<PizzaIngredientSpawner>().pizzaIngredientToSpawn.GetComponent<IngredientHitEffect>().spawnObjectOnCollision.GetComponent<PizzaIngredient>());
            Destroy(Instantiate(newToppingUI, transform.position, Quaternion.Euler(0, -90, 0)), newToppingUILifetime);
        }
    }
}
