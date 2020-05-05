using UnityEngine;

public class PizzaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pizzaPrefab = null;
    [SerializeField] private Transform pizzaSpawnPos = null;
    [SerializeField] private Transform pizzaSpawnPos2 = null;

    private bool spawnRight = false;

    public void SpawnPizza()
    {
        if (spawnRight == false)
        {
            Instantiate(pizzaPrefab, pizzaSpawnPos.position, Quaternion.identity);
            spawnRight = true;
        }

        else if(spawnRight == true)
        {
            Instantiate(pizzaPrefab, pizzaSpawnPos2.position, Quaternion.identity);
            spawnRight = false;
        }
    }
}
