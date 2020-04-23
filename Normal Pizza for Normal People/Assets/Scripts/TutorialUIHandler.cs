using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject bottomOven;
    [SerializeField] private List<GameObject> tutorialUI = new List<GameObject>();
    private GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();
        
        for(int i = 0; i < tutorialUI.Count; i++)
        {
            tutorialUI[i].SetActive(false);
        }

        StartCoroutine(RunTutorial());
    }

    /// <summary>
    /// Runs through the tutorial and waits for events to happen before continuing
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunTutorial()
    {
        tutorialUI[0].SetActive(true); //start text it up
        
        yield return new WaitUntil(() => gm.dayStarted);
        
        tutorialUI[0].SetActive(false); //start off
        tutorialUI[1].SetActive(true); //smack on
        
        yield return new WaitUntil(() => FindObjectOfType<PizzaBehaviour>());
        
        tutorialUI[1].SetActive(false); //smack off
        tutorialUI[2].SetActive(true); //grab on
        
        yield return new WaitUntil(() => FindObjectOfType<IngredientHitEffect>());
        
        tutorialUI[2].SetActive(false); //grab off
        tutorialUI[3].SetActive(true); //hover on
        
        yield return new WaitUntil(() => FindObjectOfType<PizzaBehaviour>().transform.GetChild(1).gameObject.activeSelf);
        
        tutorialUI[3].SetActive(false); //hover off
        tutorialUI[4].SetActive(true); //throw on
        
        yield return new WaitUntil(() => Customer.firstPizzaThrow);
        
        tutorialUI[4].SetActive(false); //throw off
        
        yield return new WaitUntil(() => bottomOven.activeSelf && gm.dayStarted && FindObjectOfType<PizzaBehaviour>() && FindObjectOfType<IngredientHitEffect>());
        
        tutorialUI[5].SetActive(true); // cook right on
        tutorialUI[6].SetActive(true); // cook on
        
        yield return new WaitUntil(() => bottomOven.GetComponentInChildren<OvenBehaviour>().GetIsPizzaInOven());
        
        tutorialUI[5].SetActive(false); // cook right off
        tutorialUI[6].SetActive(false); // cook off
    }
}
