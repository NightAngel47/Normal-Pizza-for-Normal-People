using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OvenBehaviour : MonoBehaviour
{
    [SerializeField] private float cookTime = 10; //time till its cooked
    [SerializeField] private float overCookTime = 20; // time till its burnt
    [SerializeField] private Material cooked;
    [SerializeField] private Material burnt;

    public Image loadingBar;
    public TextMeshProUGUI progressIndicator;
    private float timerTime;

    //private PizzaBehaviour pizza;
    private float tempTime = 0;
    private bool startOverCooking = false;
    private bool isPizzaInOven = false;

    [SerializeField]
    private Color goodGreen = new Color(0.3f, 0.83f, 0.26f);
    [SerializeField]
    private Color warnOrange = new Color(1f, 0.63f, 0f);

    private ParticleSystem ps;
    
    private enum OvenAudioStates {Cooking, PizzaDone, PizzaBurnt}
    [SerializeField] private List<AudioClip> ovenAudioClips = new List<AudioClip>();
    private AudioSource audioSource;

    private int pizzaCount = 0;
    private List<PizzaBehaviour> pizzasInOvenList = new List<PizzaBehaviour>();

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
            loadingBar.color = warnOrange;
            PlayOvenAudio(OvenAudioStates.PizzaBurnt);
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        //else it is not fully cooked yet and it checks to see if it is cooked, and if it is starts the burn part of it
        else if (pizza.cookedTime >= cookTime)
        {
            pizza.GetComponentInChildren<MeshRenderer>().material = cooked;
            pizza.isCooked = true;
            pizza.overCooking = true;
            loadingBar.color = warnOrange;
            
            if (!startOverCooking) // start overcooking
            {
                PlayOvenAudio(OvenAudioStates.PizzaDone);
                startOverCooking = true;
                tempTime = 0;
                timerTime = cookTime + 1;
                Debug.Log(timerTime);
                
                yield return new WaitWhile(() => audioSource.isPlaying);
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
    
    /*
    private void OnTriggerStay(Collider col)
    {
        //is the pizza in the oven and is it not burnt yet
        if (!col.transform.parent.TryGetComponent(out PizzaBehaviour pizza) || pizza.isBurnt) return;
        
        pizza.cookedTime += Time.deltaTime; //adds time to the amount of time the pizza has been cooked
        tempTime += Time.deltaTime; //time for fill on timer

        timerTime -= Time.deltaTime; //time displayed on timer
        string per = ((int)timerTime).ToString();

        if (pizza.cookedTime > cookTime)
        {
            loadingBar.fillAmount = tempTime / cookTime; //how much fill and assigns fill on timer
        }
        else
        {
            loadingBar.fillAmount = pizza.cookedTime / cookTime; //how much fill and assigns fill on timer
        }

        progressIndicator.text = per;

        //if it finished cooking this checks to see if it is going to burn/make it burnt
        if (pizza.overCooking && pizza.cookedTime >= overCookTime)
        {
            pizza.GetComponentInChildren<MeshRenderer>().material = burnt;
            pizza.isCooked = false;
            pizza.isBurnt = true;
            loadingBar.color = warnOrange;
            PlayOvenAudio(OvenAudioStates.PizzaBurnt);
            Invoke(nameof(BackToCookingSounds), 1f);
        }

        //else it is not fully cooked yet and it checks to see if it is cooked, and if it is starts the burn part of it
        else if (pizza.cookedTime >= cookTime)
        {
            pizza.GetComponentInChildren<MeshRenderer>().material = cooked;
            pizza.isCooked = true;
            pizza.overCooking = true;
            loadingBar.color = warnOrange;

            if (!startOverCooking)
            {
                PlayOvenAudio(OvenAudioStates.PizzaDone);
                Invoke(nameof(BackToCookingSounds), 1f);
                startOverCooking = true;
                tempTime = 0;
                timerTime = cookTime + 1;
                Debug.Log(timerTime);
            }
        }
    }
    */

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
            loadingBar.color = goodGreen;
            loadingBar.fillAmount = 0;
            tempTime = 0;
            pizza.counterTime = timerTime;
            timerTime = cookTime + 1;

            pizza.inOven = false;

            pizzaCount--;
            pizzasInOvenList.Remove(pizza);

            if(pizzaCount > 0)
            {
                StartCoroutine(PizzaInOven(pizzasInOvenList[0]));
            }
        }

        else
        {
            pizzaCount--;
            pizzasInOvenList.Remove(pizza);
        }
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
