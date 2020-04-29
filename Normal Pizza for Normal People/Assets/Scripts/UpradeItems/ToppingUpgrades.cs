using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingUpgrades : ItemUpgrades
{
    [SerializeField] private GameObject newToppingUI;

    private GameObject toppingUIInstance;
    
    private PizzaIngredientSpawner pis;

    public override void TurnOnUpgrade()
    {
        pis = gameObject.GetComponent<PizzaIngredientSpawner>();
        pis.enabled = !pis.enabled;

        if (pis.enabled)
        {
            FindObjectOfType<OrderCreation>().allPizzaIngredients.Add(gameObject.GetComponent<PizzaIngredientSpawner>().pizzaIngredientToSpawn.GetComponent<IngredientHitEffect>().spawnObjectOnCollision.GetComponent<PizzaIngredient>());
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
