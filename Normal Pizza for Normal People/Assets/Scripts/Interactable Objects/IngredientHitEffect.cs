using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngredientHitEffect : MonoBehaviour
{
    public GameObject spawnObjectOnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        var pizza = collision.collider.GetComponentInParent<PizzaBehaviour>();

        if (pizza)
        {
            ContactPoint contact = collision.contacts[0];

            float backTrackLength = 1f;
            Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
            if (collision.collider.Raycast(ray, out RaycastHit hit, 2))
            {
                var spawnPos = pizza.transform.position;
                var randRot = Quaternion.Euler(new Vector3(0, Random.Range(-180f, 180f), 0));
                var newIngredient = Instantiate(spawnObjectOnCollision, spawnPos, randRot, pizza.transform);
                pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
            }

            Destroy(gameObject);
        }
    }
}
