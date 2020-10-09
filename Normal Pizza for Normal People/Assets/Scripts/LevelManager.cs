using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Sheet1 levelData;
    
    enum CustomerLinePoses {Center, Right, Left}
    [SerializeField] private List<CustomerLineUpgrade> customerLines = new List<CustomerLineUpgrade>();
    
    enum OvensSpots {Bottom, Top}
    [SerializeField] private List<OvenUpgrade> ovens = new List<OvenUpgrade>();
    
    enum ToppingTypes {Pineapple, Laundrypod, Beetroot, Cactus, Starfish, Honeycomb}
    [SerializeField] private List<ToppingUpgrades> toppings = new List<ToppingUpgrades>();

    //TODO get selected day using save System (playerPrefs as temp)
    private void SetupLevel(int selectedDay)
    {
        // check for valid level num
        if (selectedDay == 0) return;
        
        // setup level data
        
        // setup day 
        GetComponent<GameManager>().SetDay(levelData.dataArray[selectedDay].Daynum,
                    levelData.dataArray[selectedDay].Totalcustomers,
                    levelData.dataArray[selectedDay].Daylength, 
                    levelData.dataArray[selectedDay].Scoregoal);
        
        // setup customer lines
        switch (levelData.dataArray[selectedDay].Numcustomerlines)
        {
            case 0:
                customerLines[(int) CustomerLinePoses.Right].HideItem();
                customerLines[(int) CustomerLinePoses.Left].HideItem();
                break;
            case 1:
                customerLines[(int) CustomerLinePoses.Left].HideItem();
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
        
        if (!levelData.dataArray[selectedDay].Laundrypod)
        {
            toppings[(int) ToppingTypes.Laundrypod].HideItem();
        }
        
        if (!levelData.dataArray[selectedDay].Beetroot)
        {
            toppings[(int) ToppingTypes.Beetroot].HideItem();
        }
        
        if (!levelData.dataArray[selectedDay].Cactus)
        {
            toppings[(int) ToppingTypes.Cactus].HideItem();
        }
        
        if (!levelData.dataArray[selectedDay].Starfish)
        {
            toppings[(int) ToppingTypes.Starfish].HideItem();
        }

        if (!levelData.dataArray[selectedDay].Honeycomb)
        {
            toppings[(int) ToppingTypes.Honeycomb].HideItem();
        }
        
        // setup order creation
        GetComponent<OrderCreation>().SetDayToppingValues(levelData.dataArray[selectedDay].Mintopping, 
                                                        levelData.dataArray[selectedDay].Maxtopping, 
                                                        levelData.dataArray[selectedDay].Toppingrange);
    }
}
