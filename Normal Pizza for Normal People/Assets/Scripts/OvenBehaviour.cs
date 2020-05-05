/*
 * Normal Pizza for Normal People
 * IM 389
 * OvenBehaviour
 * Sydney & Steven
 * Sydney: Created script and initial functionality of cooking pizzas and updating UI timers, made ovens work with one pizza at a time
 * Steven: Added audio sources and when to play sfx, changed cooking function to coroutine.
 * Sets up the behaviour for the oven and what to do with the pizzas when they are in the oven. Sees if pizza is cooked or burnt,
 * timers, audio, and particles for the oven.
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OvenBehaviour : MonoBehaviour
{
    [SerializeField] private float cookTime = 10; //time till its cooked
    [SerializeField] private float overCookTime = 20; // time till its burnt
    [SerializeField] private Material cooked  = null;
    [SerializeField] private Material burnt = null;

    public Image loadingBar = null;
    public TextMeshProUGUI progressIndicator = null;
    private float timerTime = 0;

    //private PizzaBehaviour pizza;
    private float tempTime = 0;
    private bool startOverCooking = false;
    private bool isPizzaInOven = false;

    public Sprite cookedPizzaImg = null;
    public Sprite burntPizzaImg = null;

    private ParticleSystem ps = null;
    
    private enum OvenAudioStates {Cooking, PizzaDone, PizzaBurnt}
    [SerializeField] private List<AudioClip> ovenAudioClips = new List<AudioClip>();
    private AudioSource audioSource = null;

    private int pizzaCount = 0;
    private readonly List<PizzaBehaviour> pizzasInOvenList = new List<PizzaBehaviour>();

    void Start()
    {
        ps = transform.parent.GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        //timerTime = cookTime + 1;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (isPizzaInOven == false)
        {
            if (!col.transform.parent.TryGetComponent(out PizzaBehaviour pizza) || pizza.isBurnt) return;

            ps.Play();
            PlayOvenAudio(OvenAudioStates.Cooking);

            if (pizza.isCooked)
            {
                tempTime = pizza.cookedTime - cookTime;
            }
            else
            {
                tempTime = pizza.cookedTime;
            }

            if (pizza.counterTime == -1)
            {
                timerTime = cookTime + 1;
            }
            else
            {
                timerTime = pizza.counterTime;
            }

            isPizzaInOven = true;
            pizzaCount++;
            pizzasInOvenList.Add(pizza);
            StartCoroutine(PizzaInOven(pizza));
        }

        else
        {
            if (!col.transform.parent.TryGetComponent(out PizzaBehaviour pizza) || pizza.isBurnt) return;
            pizzaCount++;
            pizzasInOvenList.Add(pizza);
        }
    }

    /// <summary>
    /// Cooks the pizza while in the oven
    /// </summary>
    /// <param name="pizza">The pizza being cooked</param>
    /// <returns></returns>
    private IEnumerator PizzaInOven(PizzaBehaviour pizza)
    {
        pizza.inOven = true;

        pizza.cookedTime += Time.deltaTime; //adds time to the amount of time the pizza has been cooked
        tempTime += Time.deltaTime; //time for fill on timer

        timerTime -= Time.deltaTime; //time displayed on timer
        string per = ((int)timerTime).ToString();
        progressIndicator.text = per;
        
        if (pizza.cookedTime > cookTime)
        {
            loadingBar.fillAmount = tempTime / cookTime; //how much fill and assigns fill on timer
        }
        else
        {
            loadingBar.fillAmount = pizza.cookedTime / cookTime; //how much fill and assigns fill on timer
        }
        
        //if it finished cooking this checks to see if it is going to burn/make it burnt
        if (pizza.overCooking && pizza.cookedTime >= overCookTime)
        {
            pizza.GetComponentInChildren<MeshRenderer>().material = burnt;
            pizza.isCooked = false;
            pizza.isBurnt = true;
            loadingBar.sprite = burntPizzaImg;
            PlayOvenAudio(OvenAudioStates.PizzaBurnt);
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        //else it is not fully cooked yet and it checks to see if it is cooked, and if it is starts the burn part of it
        else if (pizza.cookedTime >= cookTime)
        {
            pizza.GetComponentInChildren<MeshRenderer>().material = cooked;
            pizza.isCooked = true;
            pizza.overCooking = true;

            if (!startOverCooking) // start overcooking
            {
                PlayOvenAudio(OvenAudioStates.PizzaDone);
                startOverCooking = true;
                tempTime = 0;
                timerTime = cookTime + 1;
                //Debug.Log(timerTime);
                
                yield return new WaitWhile(() => audioSource.isPlaying);
                loadingBar.sprite = burntPizzaImg;
                if (isPizzaInOven)
                {
                    PlayOvenAudio(OvenAudioStates.Cooking);
                }
            }
        }
        
        yield return new WaitForEndOfFrame();
        if (isPizzaInOven && !pizza.isBurnt)
        {
            StartCoroutine(PizzaInOven(pizza));
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.transform.parent.TryGetComponent(out PizzaBehaviour pizza)) return;

        if (isPizzaInOven == true && col.transform.parent.GetComponent<PizzaBehaviour>().inOven == true)
        {
            //if the pizza is removed reset all variables
            
            ps.Stop();
            audioSource.Stop();

            startOverCooking = false;
            isPizzaInOven = false;
            //reset the timer when pizza leaves
            loadingBar.sprite = cookedPizzaImg;
            loadingBar.fillAmount = 0;
            tempTime = 0;
            pizza.counterTime = timerTime;
            timerTime = cookTime + 1;

            pizza.inOven = false;

            pizzaCount--;
            pizzasInOvenList.Remove(pizza);

            if(pizzaCount > 0)
            {
                StartSecondPizza(pizzasInOvenList[0]);
            }
        }

        else
        {
            pizzaCount--;
            pizzasInOvenList.Remove(pizza);
        }
    }

    public void StartSecondPizza(PizzaBehaviour pizza)
    {
        ps.Play();
        PlayOvenAudio(OvenAudioStates.Cooking);

        if (pizza.isCooked)
        {
            tempTime = pizza.cookedTime - cookTime;
        }
        else
        {
            tempTime = pizza.cookedTime;
        }

        if (pizza.counterTime == -1)
        {
            timerTime = cookTime + 1;
        }
        else
        {
            timerTime = pizza.counterTime;
        }

        isPizzaInOven = true;
        StartCoroutine(PizzaInOven(pizza));
    }

    private void PlayOvenAudio(OvenAudioStates state)
    {
        switch (state)
        {
            case OvenAudioStates.Cooking:
                audioSource.Stop();
                audioSource.clip = ovenAudioClips[(int) OvenAudioStates.Cooking];
                audioSource.loop = true;
                audioSource.Play();
                break;
            case OvenAudioStates.PizzaDone:
                audioSource.Stop();
                audioSource.clip = ovenAudioClips[(int) OvenAudioStates.PizzaDone];
                audioSource.loop = false;
                audioSource.Play();
                break;
            case OvenAudioStates.PizzaBurnt:
                audioSource.Stop();
                audioSource.clip = ovenAudioClips[(int) OvenAudioStates.PizzaBurnt];
                audioSource.loop = false;
                audioSource.Play();
                break;
        }
    }


    public bool GetIsPizzaInOven()
    {
        return isPizzaInOven;
    }
}
