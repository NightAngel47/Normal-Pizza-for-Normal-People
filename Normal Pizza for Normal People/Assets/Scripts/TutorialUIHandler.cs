using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIHandler : MonoBehaviour
{
    public List<GameObject> tutorialUI = new List<GameObject>();
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < tutorialUI.Count; i++)
        {
            tutorialUI[i].SetActive(false);
        }

        gm = GetComponent<GameManager>();
        tutorialUI[0].SetActive(true); //start text it up
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.dayStarted == true)
        {
            tutorialUI[0].SetActive(false); //start off
            tutorialUI[1].SetActive(true); //smack on
        }
        if(FindObjectOfType<PizzaBehaviour>())
        {
            tutorialUI[1].SetActive(false); //smack off
            tutorialUI[2].SetActive(true); //grab on
        }

        if(FindObjectOfType<IngredientHitEffect>())
        {
            tutorialUI[2].SetActive(false); //grab off
            tutorialUI[3].SetActive(true); //throw on
        }

        if(Customer.firstPizzaThrow == true)
        {
            tutorialUI[3].SetActive(false); //throw off
        }
    }
}
