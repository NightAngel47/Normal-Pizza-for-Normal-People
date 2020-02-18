using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem; 

public class ButtonExample1 : MonoBehaviour
{
    //private GameManager gm;
    
    public HoverButton hoverButton;

    public GameObject prefab1;

    //private AudioSource audioSource;

    private void Start()
    {
        //gm = FindObjectOfType<GameManager>();
        hoverButton.onButtonDown.AddListener(OnButtonDown);
        //pizzas = new GameObject[5];
        //audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnButtonDown(Hand hand)
    {
        StartCoroutine(SpawnPizza());
    }

    private IEnumerator SpawnPizza()
    {
        //if (!gm.inGame) yield break;
        PizzaScript[] pizzas;

        //pizzas = GameObject.FindGameObjectsWithTag("pizza");
        pizzas = FindObjectsOfType<PizzaScript>();
        //Debug.Log("za: " + pizzas.Length);
        //for (int i = 0; i < pizzas.Length; i++)
        //{
            
        //}

        if (pizzas.Length < 4)
        {
            Instantiate(prefab1, gameObject.transform.position, Quaternion.identity);
            pizzas = new PizzaScript[0];
            //audioSource.Play();
        }

        yield return null;
    }
}

