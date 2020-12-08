using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Sheet1 levelData;
    [SerializeField] private GameObject startOfDaySummary;
    [SerializeField] private GameObject starOneScore;
    [SerializeField] private GameObject starTwoScore;
    [SerializeField] private GameObject starThreeScore;

    public List<string> narrativeBodyText = new List<string>();

    enum CustomerLinePoses {Center, Right, Left}
    [SerializeField] private List<CustomerLineUpgrade> customerLines = new List<CustomerLineUpgrade>();
    
    enum OvensSpots {Bottom, Top}
    [SerializeField] private List<OvenUpgrade> ovens = new List<OvenUpgrade>();
    
    enum ToppingTypes {Pineapple, Laundrypod, Beetroot, Cactus, Starfish, Honeycomb}
    [SerializeField] private List<ToppingUpgrades> toppings = new List<ToppingUpgrades>();

    [SerializeField] private CheeseUpgrade cheesePress;
    
    public void SetupLevel(int selectedDay)
    {
        // setup day 
        GetComponent<GameManager>().SetDay(levelData.dataArray[selectedDay].Daynum,
            levelData.dataArray[selectedDay].Totalcustomers,
            levelData.dataArray[selectedDay].Daylength,
            levelData.dataArray[selectedDay].Scoregoal,
            levelData.dataArray[selectedDay].Startwogoal,
            levelData.dataArray[selectedDay].Starthreegoal);

        if (selectedDay == 0)
        {
            startOfDaySummary.SetActive(true);
        }
        startOfDaySummary.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Day " + (selectedDay + 1);
        startOfDaySummary.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = narrativeBodyText[selectedDay];
        starOneScore.GetComponent<TextMeshProUGUI>().text = levelData.dataArray[selectedDay].Scoregoal.ToString();
        starTwoScore.GetComponent<TextMeshProUGUI>().text = levelData.dataArray[selectedDay].Startwogoal.ToString();
        starThreeScore.GetComponent<TextMeshProUGUI>().text = levelData.dataArray[selectedDay].Starthreegoal.ToString();
        CustomerLine customerLine = FindObjectOfType<CustomerLine>();
        // setup customer lines
        switch (levelData.dataArray[selectedDay].Numcustomerlines)
        {
            case 0:
                customerLine.AddNewCustomerLine(customerLines[(int) CustomerLinePoses.Center].GetComponent<CustomerLinePos>());
                customerLines[(int) CustomerLinePoses.Right].HideItem();
                customerLines[(int) CustomerLinePoses.Left].HideItem();
                break;
            case 1:
                customerLine.AddNewCustomerLine(customerLines[(int) CustomerLinePoses.Center].GetComponent<CustomerLinePos>());
                customerLine.AddNewCustomerLine(customerLines[(int) CustomerLinePoses.Right].GetComponent<CustomerLinePos>());
                customerLines[(int) CustomerLinePoses.Left].HideItem();
                break;
            default:
                customerLine.AddNewCustomerLine(customerLines[(int) CustomerLinePoses.Center].GetComponent<CustomerLinePos>());
                customerLine.AddNewCustomerLine(customerLines[(int) CustomerLinePoses.Right].GetComponent<CustomerLinePos>());
                customerLine.AddNewCustomerLine(customerLines[(int) CustomerLinePoses.Left].GetComponent<CustomerLinePos>());
                break;
        }
        
        // setup ovens
        switch (levelData.dataArray[selectedDay].Numofovens)
        {
            case 0:
                ovens[(int) OvensSpots.Bottom].HideItem();
                ovens[(int) OvensSpots.Top].HideItem();                
                break;
            case 1:
                ovens[(int) OvensSpots.Top].HideItem();
                break;
        }
        
        // setup toppings
        if (!levelData.dataArray[selectedDay].Pineapple)
        {
            toppings[(int) ToppingTypes.Pineapple].HideItem();
        }
        else
        {
            toppings[(int) ToppingTypes.Pineapple].TurnOnUpgrade();
        }
        
        if (!levelData.dataArray[selectedDay].Laundrypod)
        {
            toppings[(int) ToppingTypes.Laundrypod].HideItem();
        }
        else
        {
            toppings[(int) ToppingTypes.Laundrypod].TurnOnUpgrade();
        }
        
        if (!levelData.dataArray[selectedDay].Beetroot)
        {
            toppings[(int) ToppingTypes.Beetroot].HideItem();
        }
        else
        {
            toppings[(int) ToppingTypes.Beetroot].TurnOnUpgrade();
        }
        
        if (!levelData.dataArray[selectedDay].Cactus)
        {
            toppings[(int) ToppingTypes.Cactus].HideItem();
        }
        else
        {
            toppings[(int) ToppingTypes.Cactus].TurnOnUpgrade();
        }
        
        if (!levelData.dataArray[selectedDay].Starfish)
        {
            toppings[(int) ToppingTypes.Starfish].HideItem();
        }
        else
        {
            toppings[(int) ToppingTypes.Starfish].TurnOnUpgrade();
        }

        if (!levelData.dataArray[selectedDay].Honeycomb)
        {
            toppings[(int) ToppingTypes.Honeycomb].HideItem();
        }
        else
        {
            toppings[(int) ToppingTypes.Honeycomb].TurnOnUpgrade();
        }
        
        // setup cheese press
        if (!levelData.dataArray[selectedDay].Cheesepress)
        {
            cheesePress.HideItem();   
        }

        else
        {
            cheesePress.TurnOnUpgrade();
            switch (levelData.dataArray[selectedDay].Cheeses)
            {
                case 0:
                    cheesePress.transform.GetChild(0).gameObject.SetActive(false);
                    cheesePress.transform.GetChild(1).gameObject.SetActive(false);
                    cheesePress.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case 1:
                    cheesePress.transform.GetChild(0).gameObject.SetActive(true);
                    cheesePress.transform.GetChild(1).gameObject.SetActive(false);
                    cheesePress.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case 2:
                    cheesePress.transform.GetChild(0).gameObject.SetActive(true);
                    cheesePress.transform.GetChild(1).gameObject.SetActive(true);
                    cheesePress.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                case 3:
                    cheesePress.transform.GetChild(0).gameObject.SetActive(true);
                    cheesePress.transform.GetChild(1).gameObject.SetActive(true);
                    cheesePress.transform.GetChild(2).gameObject.SetActive(true);
                    break;
            }
        }
        
        // setup order creation
        GetComponent<OrderCreation>().SetDayToppingValues(levelData.dataArray[selectedDay].Mintopping, 
                                                        levelData.dataArray[selectedDay].Maxtopping, 
                                                        levelData.dataArray[selectedDay].Toppingrange);
    }

    public void ResetLevel()
    {
        List<ItemUpgrades> allItems = new List<ItemUpgrades>();
        allItems.AddRange(customerLines);
        allItems.AddRange(ovens);
        allItems.AddRange(toppings);

        foreach (var item in allItems)
        {
            item.ShowItem();
        }

        OrderCreation orderCreation = FindObjectOfType<OrderCreation>();
        orderCreation.tierOneIngredients.Clear();
        orderCreation.tierTwoIngredients.Clear();
        orderCreation.tierThreeIngredients.Clear();
    }
}
