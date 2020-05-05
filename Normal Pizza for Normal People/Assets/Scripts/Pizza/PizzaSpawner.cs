/*
 * Normal Pizza for Normal People
 * IM 389
 * PizzaSpawner
 * Sydney & Steven
 * Sydney: Added updates so that pizzas spawn in one of two locations
 * Sets up how pizzas Spawn. When a button is hit spawn pizza is called and flip bakc and forth on where it spawns
 */

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
