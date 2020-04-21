using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingUpgrades : ItemUpgrades
{
    private PizzaIngredientSpawner pis;

    protected override void ChangeMaterial(Material changeMat)
    {
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = changeMat;
    }

    public override void TurnOnUpgrade()
    {
        pis = gameObject.GetComponent<PizzaIngredientSpawner>();
        pis.enabled = !pis.enabled;

        if (pis.enabled)
        {
            FindObjectOfType<OrderCreation>().allPizzaIngredients.Add(gameObject.GetComponent<PizzaIngredientSpawner>().pizzaIngredientToSpawn.GetComponent<IngredientHitEffect>().spawnObjectOnCollision.GetComponent<PizzaIngredient>());
        }
    }

    protected override void TurnOffPurchaseCollider()
    {
        throw new System.NotImplementedException();
    }
}
