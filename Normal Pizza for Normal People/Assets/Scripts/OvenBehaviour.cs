using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OvenBehaviour : MonoBehaviour
{
    //private GameManager gm;
    
    private bool inOven = false;
    public float cookTime = 10;
    public float overCookTime = 20;
    private float currentTime = 0;
    public Material cooked;
    public Material burnt;
    public Image loadingBar;
    public TextMeshProUGUI progressIndicator;

    private float tempTime;
    private bool overCooking = false;

    private Color goodGreen = new Color(0.3f, 0.83f, 0.26f);
    private Color warnOrange = new Color(1f, 0.63f, 0f);

    //private ParticleSystem ps;
    //private AudioSource audioSource;
    //[SerializeField]
    //private AudioClip[] audioClips; // 0 = cooking, 1 = ding, 2 = burnt 3 = first placed

    private void Start()
    {
        //gm = FindObjectOfType<GameManager>();
        //ps = gameObject.GetComponent<ParticleSystem>();
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("pizza") && col.gameObject.GetComponentInParent<PizzaScript>().isBurnt == false)//&& !gm.isPaused
        {
            //audioSource.Stop();
            //audioSource.clip = audioClips[3];
            //audioSource.loop = false;
            //audioSource.Play();

            //ps.Play();
            currentTime = 0;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        //Debug.Log(col.tag);
        if (col.gameObject.CompareTag("pizza") && col.gameObject.GetComponentInParent<PizzaScript>().isBurnt == false)// && !gm.isPaused
        { 
            inOven = true;
            currentTime += Time.deltaTime;
            int percent = ((int)currentTime / (int)cookTime) * 100;
            string per = (percent).ToString();
            loadingBar.fillAmount = currentTime / cookTime;

            //if (!audioSource.isPlaying)
            //{
            //    audioSource.clip = audioClips[0];
            //    audioSource.loop = true;
            //    audioSource.Play();
            //}

            if (overCooking == true && currentTime >= cookTime)
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

            else if (currentTime >= cookTime)
            {
                col.gameObject.GetComponentInChildren<MeshRenderer>().material = cooked;
                col.gameObject.GetComponentInParent<PizzaScript>().isCooked = true;
                currentTime = 0;
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
        if (col.gameObject.CompareTag("pizza"))//&& !gm.isPaused
        {
            //ps.Stop();
            //audioSource.Stop();
            inOven = false;
            overCooking = false;
            currentTime = 0;
            loadingBar.color = goodGreen;
            loadingBar.fillAmount = 0;
            progressIndicator.text = "Cooking";
        }
    }
}
