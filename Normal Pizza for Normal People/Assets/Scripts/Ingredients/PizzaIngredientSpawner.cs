using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PizzaIngredientSpawner : MonoBehaviour
{
    [SerializeField] public GameObject pizzaIngredientToSpawn = null;

    private bool hasIngredient = false;

    public ToppingObjectPool op = null; //object pool

    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        {
            hasIngredient = true;
        }

        if(col.GetComponentInParent<HandCollider>() && hasIngredient == false)
        {
            StartCoroutine(nameof(SpawnIngredient));
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        {
            hasIngredient = false;
        }
    }
    
    private IEnumerator SpawnIngredient()
    {
        hasIngredient = true;

        op.SpawnFromPool(pizzaIngredientToSpawn.tag, transform.position, Quaternion.identity);

        //Instantiate(pizzaIngredientToSpawn, transform.position, Quaternion.identity);

        yield return new WaitForEndOfFrame();
    }
}
