using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OvenBehaviour : MonoBehaviour
{
    //private GameManager gm;

    private bool inOven = false;
    public float cookTime = 10;
    public float overCookTime = 20;
    public Material cooked;
    public Material burnt;
    public Image loadingBar;
    public TextMeshProUGUI progressIndicator;

    private float tempTime;
    private bool overCooking = false; //is it about to be burnt

    public PizzaBehaviour pizza;

    private Color goodGreen = new Color(0.3f, 0.83f, 0.26f);
    private Color warnOrange = new Color(1f, 0.63f, 0f);

    //private ParticleSystem ps;
    //private AudioSource audioSource;
    //[SerializeField]
    //private AudioClip[] audioClips; // 0 = cooking, 1 = ding, 2 = burnt 3 = first placed
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponentInParent<PizzaBehaviour>() == true && col.gameObject.GetComponentInParent<PizzaScript>().isBurnt == false)//&& !gm.isPaused
        {
            //audioSource.Stop();
            //audioSource.clip = audioClips[3];
            //audioSource.loop = false;
            //audioSource.Play();

            //ps.Play();
            //currentTime = 0;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        //is the pizza in the oven and is it not burnt yet
        if (col.gameObject.GetComponentInParent<PizzaBehaviour>() == true && col.gameObject.GetComponentInParent<PizzaScript>().isBurnt == false)// && !gm.isPaused
        {
            pizza.cookedTime += Time.deltaTime;
            int percent = ((int)pizza.cookedTime / (int)cookTime) * 100;
            string per = (percent).ToString();
            loadingBar.fillAmount = pizza.cookedTime / cookTime;

            //if (!audioSource.isPlaying)
            //{
            //    audioSource.clip = audioClips[0];
            //    audioSource.loop = true;
            //    audioSource.Play();
            //}

            //if it finished cooking this checks to see if it is going to burn/make it burnt
            if (overCooking == true && pizza.cookedTime >= cookTime)
            {
                col.gameObject.GetComponentInChildren<MeshRenderer>().material = burnt;
                col.gameObject.GetComponentInParent<PizzaScript>().isCooked = false;
                col.gameObject.GetComponentInParent<PizzaScript>().isBurnt = true;
                loadingBar.color = warnOrange;
                progressIndicator.text = "Burnt";

                //audioSource.Stop();
                //audioSource.clip = audioClips[1];
                //audioSource.loop = false;
                //audioSource.Play();

                //audioSource.clip = audioClips[2];
                //audioSource.Play();
            }

            //else it is not fully cooked yet and it checks to see if it is going to be cooked
            else if (pizza.cookedTime >= cookTime)
            {
                col.gameObject.GetComponentInChildren<MeshRenderer>().material = cooked;
                col.gameObject.GetComponentInParent<PizzaScript>().isCooked = true;
                pizza.cookedTime = 0;
                overCooking = true;
                loadingBar.color = warnOrange;
                progressIndicator.text = "Ready";

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
        if (col.gameObject.GetComponentInParent<PizzaBehaviour>() == true)//&& !gm.isPaused
        {
            //ps.Stop();
            //audioSource.Stop();
            inOven = false;
            overCooking = false;
            //currentTime = 0;
            loadingBar.color = goodGreen;
            loadingBar.fillAmount = 0;
            progressIndicator.text = "Cooking";
        }
    }
}
