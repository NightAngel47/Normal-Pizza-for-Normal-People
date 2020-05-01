using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class Customer : MonoBehaviour
{
    private Order order;
    private GameManager gm;
    private MoneyTracker moneyTracker;
    private CustomerLine customerLine;
    private NavMeshAgent agent;
    private AudioSource audioSource;
    
    [SerializeField]
    private float startOrderTime = 90f;
    private float currentOrderTime;
    [SerializeField]
    private GameObject orderTimerUI;
    private TMP_Text orderTimerText;
    private Image orderTimerProgressBar;
    private enum OrderTimerStates {Start, Middle, Quarter, End}
    [SerializeField]
    private List<Color> orderTimerColors = new List<Color>();
    
    [SerializeField]
    private Transform ingredientUITransform;
    [SerializeField]
    private GameObject ingredientUI;

    private enum CustomerAudioStates {Walking, GoodOrder, BadOrder, OrderEndingSoon, AtCounter}
    [SerializeField]
    private List<AudioClip> customerAudioClips = new List<AudioClip>();

    [SerializeField]
    private GameObject moneyForOrderText;
    public GameObject moneyForOrderTextObject;
    
    [HideInInspector]
    public bool activeOrder;
    private bool leaving;
    private Vector3 targetLinePos;
    private Vector3 endPos;

    public static bool firstPizzaThrow = false;

    private Camera vrCam;

    // Start is called before the first frame update
    void Start()
    {
        vrCam = Camera.main;
        gm = FindObjectOfType<GameManager>();
        customerLine = gm.GetComponent<CustomerLine>();
        moneyTracker = gm.GetMoneyTracker();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        PlayCustomerAudio(CustomerAudioStates.Walking);
        
        endPos = transform.position + (Vector3.right * 14);

        //moneyForOrderTextObject = Instantiate(moneyForOrderText, targetLinePos, Quaternion.identity);
        //moneyForOrderTextObject.gameObject.SetActive(false);
        orderTimerText = orderTimerUI.GetComponentInChildren<TMP_Text>();
        orderTimerProgressBar = orderTimerUI.transform.GetChild(0).GetComponent<Image>();
        currentOrderTime = startOrderTime + 1;
        ChangeUIState();
    }

    public void SetTargetLine(Vector3 customerLinePos)
    {
        targetLinePos = customerLinePos;
    }

    private void Update()
    {
        if (!leaving)
        {
            if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, 0.5f))
            {
                if (hit.collider.TryGetComponent(out Customer customer) && !customer.leaving)
                {
                    agent.SetDestination(transform.position);
                }
            }
            else if(agent.destination != targetLinePos)
            {
                agent.SetDestination(targetLinePos);
            }
        }

        if (agent.velocity.magnitude > 0 && !audioSource.isPlaying)
        {
            PlayCustomerAudio(CustomerAudioStates.Walking);
        }
        else if (audioSource.clip == customerAudioClips[(int) CustomerAudioStates.Walking] && agent.remainingDistance <= 0)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LineStart"))
        {
            activeOrder = true;
            transform.rotation = other.transform.rotation;
            ChangeUIState();
            PlayCustomerAudio(CustomerAudioStates.AtCounter);
            StartCoroutine(OrderTimerCountDown());
        }
        
        if (other.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        {
            moneyTracker.CustomerChangeMoney(CheckDeliveredPizza(pizza));
            Destroy(pizza.gameObject);
            activeOrder = false;
            CustomerLeave();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LineStart") && transform.rotation == other.transform.rotation)
        {
            transform.rotation = other.transform.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LineStart"))
        {
            activeOrder = false;
            ChangeUIState();
        }
    }

    /// <summary>
    /// Sets the customer's order when spawned.
    /// </summary>
    /// <param name="customerOrder">The order that the customer will have</param>
    public void SetOrder(Order customerOrder)
    {
        order = customerOrder;
        DisplayOrder();
    }

    /// <summary>
    /// Calculates the number of unique ingredients.
    /// Instantiates ingredient ui for each unique ingredient with ingredient count.
    /// </summary>
    private void DisplayOrder()
    {
        //TODO decouple UI from customer
        // get unique ingredients
        List<PizzaIngredient> uniqueIngredients = new List<PizzaIngredient>();
        foreach (var ingredient in order.GetOrderIngredients().Where(ingredient => !uniqueIngredients.Contains(ingredient)))
        {
            uniqueIngredients.Add(ingredient);
        }

        // for each unique order ingredient
        foreach (var ingredient in uniqueIngredients)
        {
            // get unique ingredient count
            int uniqueIngredientCount = order.GetOrderIngredients().Count(orderIngredient => ingredient == orderIngredient);

            // instantiate UI
            var newIngredient = Instantiate(ingredientUI, ingredientUITransform.position, ingredientUITransform.rotation, ingredientUITransform);
            
            // update text with info
            var ingredientTexts = newIngredient.GetComponentsInChildren<TMP_Text>();
            ingredientTexts[0].text = ingredient.GetIngredientName();
            ingredientTexts[1].text = "x" + uniqueIngredientCount;

            newIngredient.GetComponentInChildren<Image>().sprite = ingredient.GetIngredientIcon();
        }
    }

    private IEnumerator OrderTimerCountDown()
    {
        orderTimerText.text = ""+(int)currentOrderTime;
        orderTimerProgressBar.fillAmount = currentOrderTime / startOrderTime;
        yield return new WaitForEndOfFrame();
        currentOrderTime -= Time.deltaTime;
        if (currentOrderTime > 0)
        {
            if (currentOrderTime / startOrderTime >= 0.66f)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.Start];
            }
            
            if (currentOrderTime / startOrderTime <= .66f)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.Middle];
            }
            
            if (currentOrderTime / startOrderTime <= .33f)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.Quarter];
            }

            if (currentOrderTime <= 10)
            {
                orderTimerProgressBar.color = orderTimerColors[(int) OrderTimerStates.End];
            
                if (!audioSource.isPlaying)
                {
                    PlayCustomerAudio(CustomerAudioStates.OrderEndingSoon);
                }
            }
            
            StartCoroutine(OrderTimerCountDown());
        }
        else
        {
            moneyTracker.CustomerChangeMoney((int) -startOrderTime);
            PlayCustomerAudio(CustomerAudioStates.BadOrder);
            activeOrder = false;
            CustomerLeave();
        }
    }

    /// <summary>
    /// Checks if the delivered pizza is valid. Returns money earned or lost based on if pizza was correct.
    /// </summary>
    /// <param name="pizza">The pizza being checked.</param>
    /// <returns>If true: it returns the amount of money the pizza earned, else: it returns the amount of money lost.</returns>
    private int CheckDeliveredPizza(PizzaBehaviour pizza)
    {
        //TODO add topping tiers
        float deliveredPizzaMoney = moneyTracker.basePizzaProfit + order.GetOrderIngredients().Count * moneyTracker.tier1Toppings;

        firstPizzaThrow = true;

        int pizzaProfit;
        if (pizza.GetIngredientsOnPizza().Count == order.GetOrderIngredients().Count)
        {
            var tempOrderList = order.GetOrderIngredients();

            for (int i = 0; i < pizza.GetIngredientsOnPizza().Count; ++i)
            {
                for (int j = 0; j < tempOrderList.Count; ++j)
                {
                    if (pizza.GetIngredientsOnPizza()[i].GetIngredientName() == tempOrderList[j].GetIngredientName())
                    {
                        tempOrderList.RemoveAt(j);
                        break;
                    }
                }
            }

            if (tempOrderList.Count <= 0)
            {
                // first 2 days don't need oven because there is no oven to cook pizzas till day 3
                if (gm.currentGameDay.dayNum > 2 && (pizza.isBurnt || !pizza.isCooked))
                {
                    PlayCustomerAudio(CustomerAudioStates.BadOrder);
                    ShowMoneyAmount(pizzaProfit = (int) -deliveredPizzaMoney / 2);
                    return pizzaProfit;
                }
                
                if (pizza.isCooked)
                {
                    deliveredPizzaMoney += (int) moneyTracker.cookedBonus;
                }
                //TODO add other bonuses
                
                PlayCustomerAudio(CustomerAudioStates.GoodOrder);
                ShowMoneyAmount(pizzaProfit = (int) (deliveredPizzaMoney * (1 + currentOrderTime / startOrderTime)));
                return pizzaProfit;
            }
        }

        PlayCustomerAudio(CustomerAudioStates.BadOrder);
        ShowMoneyAmount(pizzaProfit = (int) -deliveredPizzaMoney / 2);
        return pizzaProfit;
    }

    public void CustomerLeave()
    {
        if (leaving || activeOrder) return;
        
        agent.SetDestination(endPos);
        leaving = true;
        customerLine.IncreaseCustomersServed();
        Invoke(nameof(CallTheNextCustomer), 5f);
        Destroy(gameObject, 10f);
    }

    private void CallTheNextCustomer()
    {
        customerLine.CustomerServed();
    }

    private void ChangeUIState()
    {
        ingredientUITransform.gameObject.SetActive(!ingredientUITransform.gameObject.activeSelf);
        orderTimerUI.SetActive(!orderTimerUI.activeSelf);
    }

    private void ShowMoneyAmount(int amount)
    {
        moneyForOrderTextObject = Instantiate(moneyForOrderText, gameObject.transform.position, Quaternion.identity);
        moneyForOrderTextObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "$" + amount;
        moneyForOrderTextObject.transform.LookAt(vrCam.transform);
        moneyForOrderTextObject.gameObject.SetActive(true);
        ingredientUITransform.gameObject.SetActive(false);

        Destroy(ingredientUITransform.transform.parent.gameObject);
        Destroy(moneyForOrderTextObject, 3);
        
    }

    private void PlayCustomerAudio(CustomerAudioStates state)
    {
        switch (state)
        {
            case CustomerAudioStates.Walking:
                audioSource.clip = customerAudioClips[(int) CustomerAudioStates.Walking];
                audioSource.loop = true;
                audioSource.Play();
                break;
            case CustomerAudioStates.GoodOrder:
                audioSource.clip = customerAudioClips[(int) CustomerAudioStates.GoodOrder];
                audioSource.loop = false;
                audioSource.Play();
                break;
            case CustomerAudioStates.BadOrder:
                audioSource.clip = customerAudioClips[(int) CustomerAudioStates.BadOrder];
                audioSource.loop = false;
                audioSource.Play();
                break;
            case CustomerAudioStates.OrderEndingSoon:
                audioSource.clip = customerAudioClips[(int) CustomerAudioStates.OrderEndingSoon];
                audioSource.loop = false;
                audioSource.Play();
                break;
            case CustomerAudioStates.AtCounter:
                audioSource.clip = customerAudioClips[(int) CustomerAudioStates.AtCounter];
                audioSource.loop = false;
                audioSource.Play();
                break;
        }
    }
}