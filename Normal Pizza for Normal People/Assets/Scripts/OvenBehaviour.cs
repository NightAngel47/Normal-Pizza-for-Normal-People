using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OvenBehaviour : MonoBehaviour
{
    //private GameManager gm;

    //private bool inOven = false;
    public float cookTime = 10; //time till its cooked
    public float overCookTime = 20; // time till its burnt
    public Material cooked;
    public Material burnt;

    public Image loadingBar;
    public TextMeshProUGUI progressIndicator;
    private float timerTime;

    private PizzaBehaviour pizza;
    private float tempTime = 0;

    private bool changeOnce = false;

    private Color goodGreen = new Color(0.3f, 0.83f, 0.26f);
    private Color warnOrange = new Color(1f, 0.63f, 0f);

    private ParticleSystem ps;

    //private ParticleSystem ps;
    //private AudioSource audioSource;
    //[SerializeField]
    //private AudioClip[] audioClips; // 0 = cooking, 1 = ding, 2 = burnt 3 = first placed
    // Start is called before the first frame update

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        timerTime = cookTime + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        //if (col.gameObject.GetComponentInParent<PizzaBehaviour>() == true && col.gameObject.GetComponentInParent<PizzaScript>().isBurnt == false)//&& !gm.isPaused
        //{
        //    //audioSource.Stop();
        //    //audioSource.clip = audioClips[3];
        //    //audioSource.loop = false;
        //    //audioSource.Play();

        //    //ps.Play();
        //    //currentTime = 0;
        //}

        if (col.gameObject.GetComponentInParent<PizzaBehaviour>() == true && col.gameObject.GetComponentInParent<PizzaBehaviour>().isBurnt == false)
        {
            //ps.Play();

            if(col.gameObject.GetComponentInParent<PizzaBehaviour>().isCooked == true)
            {
                tempTime = col.gameObject.GetComponentInParent<PizzaBehaviour>().cookedTime - cookTime;
            }

            else
            {
                tempTime = col.gameObject.GetComponentInParent<PizzaBehaviour>().cookedTime;
            }
            
            if(col.gameObject.GetComponentInParent<PizzaBehaviour>().counterTime == -1)
            {
                Debug.Log("here3");
                timerTime = cookTime + 1;
            }

            else
            {
                timerTime = col.gameObject.GetComponentInParent<PizzaBehaviour>().counterTime;
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        //is the pizza in the oven and is it not burnt yet
        if (col.transform.parent.TryGetComponent(out PizzaBehaviour pizza) && pizza.isBurnt == false)// && !gm.isPaused
        {
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
            

            //if (!audioSource.isPlaying)
            //{
            //    audioSource.clip = audioClips[0];
            //    audioSource.loop = true;
            //    audioSource.Play();
            //}

            //if it finished cooking this checks to see if it is going to burn/make it burnt
            if (pizza.overCooking == true && pizza.cookedTime >= overCookTime)
            {
                pizza.GetComponentInChildren<MeshRenderer>().material = burnt;
                pizza.isCooked = false;
                pizza.isBurnt = true;
                loadingBar.color = warnOrange;
                //progressIndicator.text = "Burnt";

                //audioSource.Stop();
                //audioSource.clip = audioClips[1];
                //audioSource.loop = false;
                //audioSource.Play();

                //audioSource.clip = audioClips[2];
                //audioSource.Play();
            }

            //else it is not fully cooked yet and it checks to see if it is cooked, and if it is starts the burn part of it
            else if (pizza.cookedTime >= cookTime)
            {
                col.gameObject.GetComponentInChildren<MeshRenderer>().material = cooked;
                pizza.isCooked = true;
                pizza.overCooking = true;
                loadingBar.color = warnOrange;

                if (changeOnce == false)
                {
                    changeOnce = true;
                    tempTime = 0;
                    timerTime = cookTime + 1;
                    Debug.Log(timerTime);
                }

                //audioSource.Stop();
                //audioSource.clip = audioClips[1];
                //audioSource.loop = false;
                //audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        //if the pizza is removed reset all variables
        if (col.gameObject.GetComponentInParent<PizzaBehaviour>())//&& !gm.isPaused
        {
            //ps.Stop();
            //audioSource.Stop();
            //inOven = false;
            //currentTime = 0;

            //reset the timer when pizza leaves
            loadingBar.color = goodGreen;
            loadingBar.fillAmount = 0;
            tempTime = 0;
            col.gameObject.GetComponentInParent<PizzaBehaviour>().counterTime = timerTime;
            timerTime = cookTime + 1;
            
        }
    }
}
