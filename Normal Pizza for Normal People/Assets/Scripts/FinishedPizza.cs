using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedPizza : MonoBehaviour
{
    private GameManager gm;
    private ParticleSystem ps;
    ParticleSystem.MainModule ma;

    public static GameObject ticketRef;
    public static GameObject gRef;
    private bool ticketSecond = false;

    private List<ToppingOrderUI> toppingUI;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] audioClips; //0 bad pizza 1 good pizza 2 bad money

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
        ps = GetComponent<ParticleSystem>();
        ma = ps.main;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(ticketSecond == true && OrderTicketScript.deliveryTicket != null)
        {
            ticketSecond = false;
            GameObject.FindGameObjectWithTag("deliveredPizza").gameObject.GetComponentInParent<PizzaScript>().ticket = OrderTicketScript.deliveryTicket;
            ticketRef = GameObject.FindGameObjectWithTag("deliveredPizza").gameObject.GetComponentInParent<PizzaScript>().ticket;
            gRef = GameObject.FindGameObjectWithTag("deliveredPizza").gameObject;
            PizzaCheck(ticketRef, gRef);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("pizza"))
        {
            GameObject g = col.gameObject;
            col.gameObject.tag = "deliveredPizza";

            if (OrderTicketScript.deliveryTicket != null)
            {
                col.gameObject.GetComponentInParent<PizzaScript>().ticket = OrderTicketScript.deliveryTicket;

                PizzaCheck(col.gameObject.GetComponentInParent<PizzaScript>().ticket, g);
                ticketRef = col.gameObject.GetComponentInParent<PizzaScript>().ticket;
                gRef = g;

                
            }

            else
            {
                ticketSecond = true;
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        //if (col.gameObject.CompareTag("pizza"))
        //{
        //    GameObject g = col.gameObject;
        //    col.gameObject.tag = "deliveredPizza";

        //    if (OrderTicketScript.deliveryTicket != null)
        //    {
        //        col.gameObject.GetComponentInParent<PizzaScript>().ticket = OrderTicketScript.deliveryTicket;

        //        PizzaCheck(col.gameObject.GetComponentInParent<PizzaScript>().ticket, g);
        //        ticketRef = col.gameObject.GetComponentInParent<PizzaScript>().ticket;
        //        gRef = g;


        //    }

        //    else
        //    {
        //        ticketSecond = true;
        //    }
        //}
    }

    public void PizzaCheck(GameObject temp, GameObject g)
    {
        GameObject ticket = temp;
        int topCount = 0;
        int topCheck = 0;

        toppingUI = new List<ToppingOrderUI>(ticket.GetComponentsInChildren<ToppingOrderUI>());
        //Debug.Log(toppingUI.Count);

        for (int i = 0; i < toppingUI.Count; i++) //goes through each topping to make sure there is enough of it
        {
            topCount = 0;
            int amount = ticket.GetComponent<OrderTicketBehaviour>().toppingSelectedAmounts[i];
            string name = ticket.GetComponent<OrderTicketBehaviour>().toppingSelectedNames[i];
            //Debug.Log(amount);
            //Debug.Log(name);

            for (int j = 0; j < g.transform.childCount; j++)
            {
                if ((name + "(Clone)") == g.transform.GetChild(j).name)
                {
                    topCount++;
                }
            }

            if (topCount == amount) //one of three toppings passed 
            {
                topCheck++;
            }
        }

        

        if (topCheck == toppingUI.Count && g.GetComponentInParent<PizzaScript>().isCooked == true)
        {
            ma.startColor = Color.green;
            ps.Play();

            audioSource.Stop();
            audioSource.clip = audioClips[1];
            audioSource.loop = false;
            audioSource.Play();

            gm.MoneyChanged((int)(ticket.GetComponent<OrderTicketBehaviour>().timeLeftUI.currentTimeLeft * gm.moneyBaseAmount));
            ticket.GetComponent<OrderTicketBehaviour>().startLockPos.isOpen = true;

            Destroy(ticket);
            Destroy(g.transform.parent.gameObject);
        }

        else
        {
            ma.startColor = Color.red;
            ps.Play();

            audioSource.Stop();
            audioSource.clip = audioClips[0];
            audioSource.loop = false;
            audioSource.Play();

            gm.MoneyChanged((int)(ticket.GetComponent<OrderTicketBehaviour>().timeLeftUI.totalOrderTime * gm.moneyTicketExpiredAmount));

            audioSource.Stop();
            audioSource.clip = audioClips[2];
            audioSource.loop = false;
            audioSource.Play();

            ticket.GetComponent<OrderTicketBehaviour>().startLockPos.isOpen = true;

            Destroy(ticket);
            Destroy(g.transform.parent.gameObject);
        }

        OrderTicketScript.deliveryTicket = null;
    }

    private void OnTriggerExit(Collider col)
    {
        col.gameObject.tag = "pizza";
        //deliveredPizza.tag = "pizza";
    }
}
