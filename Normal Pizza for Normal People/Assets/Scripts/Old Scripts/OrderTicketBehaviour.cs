using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderTicketBehaviour : MonoBehaviour
{
    private GameManager gm;
    
    [SerializeField, Range(3, 15), Tooltip("The maximum total number of toppings for the ticket")]
    private int maxToppingAmount = 6;

    [SerializeField, Tooltip("Transform to spawn UI at")]
    private Transform uiTransform;
    
    [SerializeField, Tooltip("The prefab for the topping UI. Used for creating ticket")]
    private GameObject toppingUIPrefab;
    /// <summary>
    /// UI to display toppings. Toppings are 0-2 top to bottom
    /// </summary>
    private List<ToppingOrderUI> toppingUI;
    /// <summary>
    /// The name of the Topping at the index
    /// </summary>
    public readonly List<string> toppingSelectedNames = new List<string>();
    /// <summary>
    /// The icon of the Topping at the index
    /// </summary>
    private readonly List<Sprite> toppingSelectedIcons = new List<Sprite>();
    /// <summary>
    /// The amount of the Topping at the index
    /// </summary>
    public readonly List<int> toppingSelectedAmounts = new List<int>();

    [SerializeField, Tooltip("The prefab for the time left UI. Used for creating ticket")]
    private GameObject timeLeftUIPrefab;
    /// <summary>
    /// The UI for the time left
    /// </summary>
    [HideInInspector]
    public TimeLeftUI timeLeftUI;
    [SerializeField, Range(20, 120), Tooltip("The minimum time for an order")]
    private float orderTimeMin = 60f;
    [SerializeField, Range(40, 240), Tooltip("The maximum time for an order")]
    private float orderTimeMax = 90f;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;

    /// <summary>
    /// The starting lock pos that needs to be reset when finished
    /// </summary>
    [HideInInspector]
    public OrderTicketLockPosBehaviour startLockPos;

    /// <summary>
    /// Spawns UI elements based on topping count and adds to respective variables
    /// </summary>
    private void SpawnUIElements()
    {
        int toppingCount = Random.Range(1, 4);
        
        // spawn UI
        for(int i = 0; i < toppingCount; ++i)
        {
            Instantiate(toppingUIPrefab, uiTransform);
        }

        GameObject newTimeLeftUI = Instantiate(timeLeftUIPrefab, uiTransform);
        timeLeftUI = newTimeLeftUI.GetComponent<TimeLeftUI>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.Stop();
        audioSource.clip = audioClips[0];
        audioSource.loop = false;
        audioSource.Play();

        // spawn UI elements
        SpawnUIElements();
        
        // get references
        gm = FindObjectOfType<GameManager>();
        toppingUI = new List<ToppingOrderUI>(GetComponentsInChildren<ToppingOrderUI>());
        
        // set up order toppings
        int currentToppingAmount = 0;
        for(int i = 0; i < toppingUI.Count; ++i)
        {
            CreateOrder(i, ref currentToppingAmount);
            toppingUI[i].SetUI(toppingSelectedIcons[i], toppingSelectedNames[i], toppingSelectedAmounts[i]);
        }
        
        // random time
        float randomTime = Random.Range(orderTimeMin, orderTimeMax);
        timeLeftUI.SetTime(randomTime);
        
        // destroy the order when past order time
        Destroy(gameObject, randomTime);

        
    }

    /// <summary>
    /// Creates random order for each topping
    /// </summary>
    /// <param name="i">Current topping index</param>
    /// <param name="currentToppingAmount">Current amount of toppings for this ticket</param>
    void CreateOrder(int i, ref int currentToppingAmount)
    {
        // random toppings
        int randomTopping = Random.Range(0, gm.toppingNames.Length);
        
        if(i == 0) // first topping is whatever
        {
            toppingSelectedNames.Add(gm.toppingNames[randomTopping]);
            toppingSelectedIcons.Add(gm.toppingIcons[randomTopping]);
        }
        else // other two toppings
        {
            int isSame = 0;

            for(int j = 0; j < i; j++)
            {
                if(gm.toppingNames[randomTopping].Equals(toppingSelectedNames[j]))
                {
                    isSame++;
                }
            }

            if(isSame > 0)
            {
                CreateOrder(i, ref currentToppingAmount);
            }
            else
            {
                toppingSelectedNames.Add(gm.toppingNames[randomTopping]);
                toppingSelectedIcons.Add(gm.toppingIcons[randomTopping]);
            }
        }
        
        // random amounts 1 to max amount of toppings - current amount of toppings - 1 for each topping type + 1 for adjustment
        int randomAmount = Random.Range(1, maxToppingAmount - currentToppingAmount - toppingUI.Count + 1);
        currentToppingAmount = randomAmount;
        toppingSelectedAmounts.Add(randomAmount);
    }

    /// <summary>
    /// Sets the startLockPos
    /// </summary>
    public void SetInitialLockPos(OrderTicketLockPosBehaviour spawnLockPos)
    {
        startLockPos = spawnLockPos;
    }

    private void OnDestroy()
    {
        gm.openOrderPos--;
        if (timeLeftUI.currentTimeLeft <= 0)
        {        
            gm.MoneyChanged((int) -(timeLeftUI.totalOrderTime * gm.moneyTicketExpiredAmount));
        }
        startLockPos.isOpen = true;
    }
}
