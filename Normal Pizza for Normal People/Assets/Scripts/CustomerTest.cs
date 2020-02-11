using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTest : MonoBehaviour
{
    List<GameObject> ingredients = new List<GameObject>();

    public GameObject topping1;

    public void Start()
    {
        ingredients.Add(topping1);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(ingredients.Count == col.gameObject.GetComponent<PizzaScript>().ingredientList.Count)
        {
            foreach(GameObject g in ingredients)
            {
                foreach(GameObject k in col.gameObject.GetComponent<PizzaScript>().ingredientList)
                {
                    if(g.name == k.name)
                    {
                        ingredients.Remove(g);
                        col.gameObject.GetComponent<PizzaScript>().ingredientList.Remove(k);
                    }
                }
            }

            if(ingredients.Count == col.gameObject.GetComponent<PizzaScript>().ingredientList.Count) //both should be empty if done right
            {
                //pizza delivered right
            }

            else
            {
                //bad pizza
            }
        }

        else
        {
            //not same size not same pizza
        }
    }
}
