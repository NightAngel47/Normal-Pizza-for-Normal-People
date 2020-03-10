using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngredientHitEffect : MonoBehaviour
{
    public GameObject spawnObjectOnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        var pizza = collision.collider.GetComponentInParent<PizzaBehaviour>();

        if (!pizza) return;
        ContactPoint contact = collision.contacts[0];

        float backTrackLength = 1f;
        Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
        if (collision.collider.Raycast(ray, out RaycastHit hit, 2))
        {
            var pizzaTransform = pizza.transform;
            var spawnPos = pizzaTransform.position;
            var newIngredient = Instantiate(spawnObjectOnCollision, spawnPos, pizzaTransform.rotation, pizzaTransform);
            newIngredient.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0));
            pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
        }

        Destroy(gameObject);
    }
}
