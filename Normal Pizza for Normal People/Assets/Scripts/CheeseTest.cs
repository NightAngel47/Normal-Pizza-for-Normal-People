using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CheeseTest : MonoBehaviour
{
    public GameObject cheeseToSpawn;
    public GameObject linear;
    public GameObject cheeseOne;
    public GameObject cheeseTwo;
    public GameObject cheeseThree;

    public GameObject cheeseParent;
    public Material selected;
    public Material notSelected;

    public void Start()
    {
        cheeseToSpawn = cheeseOne;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        { 
            if (linear.GetComponent<LinearMapping>().value >= .7f)
            {
                var spawnPos = pizza.transform.position;
                var newIngredient = Instantiate(cheeseToSpawn, spawnPos, pizza.transform.rotation, pizza.transform);
                newIngredient.transform.Rotate(Vector3.up, Random.Range(-180, 180), Space.Self);
                pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
            }
        }
    }

    public void SelectCheeseOne()
    {
        cheeseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = selected;
        cheeseParent.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = notSelected;
        cheeseParent.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = notSelected;
        cheeseToSpawn = cheeseOne;
    }

    public void SelectCheeseTwo()
    {
        cheeseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = notSelected;
        cheeseParent.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = selected;
        cheeseParent.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = notSelected;
        cheeseToSpawn = cheeseTwo;
    }

    public void SelectCheeseThree()
    {
        cheeseParent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = notSelected;
        cheeseParent.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = notSelected;
        cheeseParent.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = selected;
        cheeseToSpawn = cheeseThree;
    }
}
