using UnityEngine;

public class IngredientHitEffect : MonoBehaviour
{
    public GameObject spawnObjectOnCollision;
    public Material mat;
    public bool inHand = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (inHand) return;
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
            newIngredient.transform.Rotate(Vector3.up, Random.Range(-180, 180), Space.Self);
            pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
        }

        gameObject.SetActive(false);
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (inHand) return;
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
            newIngredient.transform.Rotate(Vector3.up, Random.Range(-180, 180), Space.Self);
            pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
        }

        gameObject.SetActive(false);
    }

    public void ChangeInHand(bool state)
    {
        inHand = state;
    }
}
