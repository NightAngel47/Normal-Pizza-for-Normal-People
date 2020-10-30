/*
 * Normal Pizza for Normal People
 * IM 389
 * OrderCreation
 * Steven:
 * Handles creating all of the orders for each customer based on what ingredients are available
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderCreation : MonoBehaviour
{
    public List<PizzaIngredient> tierOneIngredients = new List<PizzaIngredient>();
    public List<PizzaIngredient> tierTwoIngredients = new List<PizzaIngredient>();
    public List<PizzaIngredient> tierThreeIngredients = new List<PizzaIngredient>();
    public List<PizzaIngredient> cheeseIngredients = new List<PizzaIngredient>();
    //[SerializeField] private int minIngredientsPerOrder = 1;
    //[SerializeField] private int maxIngredientsPerOrder = 3;
    [SerializeField] private int minTotalToppingsPerOrder = 0;
    [SerializeField] private int maxTotalToppingsPerOrder = 0;
    [SerializeField] private int rangeLevel = 0; //1,2,3 low,mid,high
    [SerializeField] private List<TotalToppingCurves> toppingTotalRanges = new List<TotalToppingCurves>();
    [SerializeField] private List<MaxIngredientAmount> ingredientMax = new List<MaxIngredientAmount>();
    private PizzaIngredient pickedTopping;
    private bool dont = false;
    private int tiersAvailable;
    private int differentToppings;

    [Serializable]
    public struct TotalToppingCurves
    {
        public List<int> low;
        public List<int> mid;
        public List<int> high;
    }

    [Serializable]
    public struct MaxIngredientAmount //the max number of how much one ingredient can be on the pizza, per tier
    {
        public int tierOne;
        public int tierTwo;
        public int tierThree;
    }

    public void SetDayToppingValues(int minTotal, int maxTotal, int ranLevel)
    {
        minTotalToppingsPerOrder = minTotal;
        maxTotalToppingsPerOrder = maxTotal;
        rangeLevel = ranLevel;
    }

    /// <summary>
    /// Generates the orders for the number of customers
    /// </summary>
    /// <param name="numOfCustomers">The number of orders to generates</param>
    /// <returns>Returns list of orders for customers</returns>
    public List<Order> GenerateOrders(int numOfCustomers)
    {
        List<Order> orders = new List<Order>(numOfCustomers);
        
        for (int i = 0; i < orders.Capacity; ++i)
        {
            orders.Add(new Order(GenerateOrderIngredients()));
        }
        
        return orders;
    }

    private List<PizzaIngredient> GenerateOrderIngredients()
    {
        List<PizzaIngredient> ingredients = new List<PizzaIngredient>(); //list of ingredients on the pizza
        int toppingRange = maxTotalToppingsPerOrder - minTotalToppingsPerOrder;
        List<List<int>> rangeLevelLists = new List<List<int>> { toppingTotalRanges[toppingRange].low, toppingTotalRanges[toppingRange].mid, toppingTotalRanges[toppingRange].high };
        int totalToppingsPerPizza = UnityEngine.Random.Range(1, 101); //random number 1-100 to decide how many on pizza

        for(int i = 0; i <= toppingRange; i++)
        {
            if(i == 0)
            {
                if(totalToppingsPerPizza >= 0 && totalToppingsPerPizza <= rangeLevelLists[rangeLevel][i])
                {
                    totalToppingsPerPizza = i + minTotalToppingsPerOrder;
                    break;
                }
            }

            else
            {
                if (totalToppingsPerPizza > rangeLevelLists[rangeLevel][i - 1] && totalToppingsPerPizza <= rangeLevelLists[rangeLevel][i])
                {
                    totalToppingsPerPizza = i + minTotalToppingsPerOrder;
                    break;
                }
            }
        }

        int totalToppingTemp = totalToppingsPerPizza;

        List<PizzaIngredient> oneTemp = new List<PizzaIngredient>(tierOneIngredients);
        List<PizzaIngredient> twoTemp = new List<PizzaIngredient>(tierTwoIngredients);
        List<PizzaIngredient> threeTemp = new List<PizzaIngredient>(tierThreeIngredients);

        int tierRand;
        int amount = 0;
        int forLoopRuns = tierOneIngredients.Count + tierTwoIngredients.Count + tierThreeIngredients.Count;
        differentToppings = UnityEngine.Random.Range(1, ((oneTemp.Count + twoTemp.Count + threeTemp.Count) + 1));
        //Debug.Log(differentToppings);

        if(oneTemp.Count > 0 && twoTemp.Count == 0 && threeTemp.Count == 0)
        {
            tiersAvailable = 0;
        }
        if (oneTemp.Count > 0 && twoTemp.Count > 0 && threeTemp.Count == 0)
        {
            tiersAvailable = 1;
        }
        if (oneTemp.Count > 0 && twoTemp.Count > 0 && threeTemp.Count > 0)
        {
            tiersAvailable = 2;
        }
        if (oneTemp.Count == 0 && twoTemp.Count > 0 && threeTemp.Count == 0)
        {
            tiersAvailable = 3;
        }

        for (int i = 0; i < differentToppings; ++i) //Potential max amount of different ingredients i.e. 6 toppings 6 different options
        {
            dont = false;

            switch(tiersAvailable)
            {
                case 0: //just tier 1
                    oneTemp = PickTopping(oneTemp); //which tier one topping
                    amount = PickToppingAmount(totalToppingsPerPizza, i); //how many of that tier one topping
                    break;
                case 1: //just tier 1 and 2
                    tierRand = UnityEngine.Random.Range(0, 100);

                    if (tierRand < 50) //tier 1
                    {
                        oneTemp = PickTopping(oneTemp); //which tier one topping
                        amount = PickToppingAmount(totalToppingsPerPizza, i); //how many of that tier one topping
                    }

                    if (tierRand >= 50 && tierRand <= 99) //tier 2
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(totalToppingsPerPizza, i);
                    }
                    break;
                case 2: //all 3 tiers
                    tierRand = UnityEngine.Random.Range(0, 100);

                    if (tierRand < 30) //tier 1
                    {
                        oneTemp = PickTopping(oneTemp); //which tier one topping
                        amount = PickToppingAmount(totalToppingsPerPizza, i); //how many of that tier one topping
                    }

                    if (tierRand >= 30 && tierRand < 60) //tier 2
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(totalToppingsPerPizza, i);
                    }

                    if (tierRand >= 60 && tierRand <= 99) //tier 3
                    {
                        threeTemp = PickTopping(threeTemp);
                        amount = PickToppingAmount(totalToppingsPerPizza, i);
                    }
                    break;
                case 3: //just tier 2
                    twoTemp = PickTopping(twoTemp);
                    amount = PickToppingAmount(totalToppingsPerPizza, i);
                    break;
                case 4:
                    threeTemp = PickTopping(threeTemp);
                    amount = PickToppingAmount(totalToppingsPerPizza, i);
                    break;
                case 5:
                    tierRand = UnityEngine.Random.Range(0, 100);
                    if (tierRand < 50)
                    {
                        twoTemp = PickTopping(twoTemp);
                    }

                    if (tierRand >= 50 && tierRand <= 99)
                    {
                        threeTemp = PickTopping(threeTemp);
                    }

                    amount = PickToppingAmount(totalToppingsPerPizza, i);
                    break;
                case 6:
                    tierRand = UnityEngine.Random.Range(0, 100);
                    if (tierRand < 50)
                    {
                        oneTemp = PickTopping(oneTemp);
                    }

                    if (tierRand >= 50 && tierRand <= 99)
                    {
                        threeTemp = PickTopping(threeTemp);
                    }

                    amount = PickToppingAmount(totalToppingsPerPizza, i);
                    break;
            }

            if(amount == 0)
            {
                amount = totalToppingTemp;
            }

            if (totalToppingTemp - amount < 0) //the amount of toppings to be added is greater than what is available on the pizza
            {
                amount = totalToppingTemp;
            }
            
            totalToppingTemp -= amount;

            for (int k = 0; k < amount; k++)
            {
                ingredients.Add(pickedTopping);
            }


            //if(i == forLoopRuns - 1 && totalToppingTemp != 0)
            //{
            //    i = 0;
            //}
        }

        if (FindObjectOfType<LevelManager>().levelData.dataArray[LevelSelect.selectedLevel].Cheesepress == true)
        {
            int cheeses = FindObjectOfType<LevelManager>().levelData.dataArray[LevelSelect.selectedLevel].Cheeses;
            int randCheese;

            switch (cheeses)
            {
                case 0:
                    // no cheese
                    break;
                case 1:
                    randCheese = UnityEngine.Random.Range(0, 100);

                    if (randCheese <= 65) // tier 1 cheese
                    {
                        ingredients.Add(cheeseIngredients[0]);
                    }

                    if (randCheese > 65 && randCheese <= 99) //no cheese
                    {
                        // no cheese
                    }
                    break;
                case 2:
                    randCheese = UnityEngine.Random.Range(0, 100);

                    if (randCheese <= 40) // no cheese
                    {
                        // no cheese
                    }

                    if (randCheese > 40 && randCheese <= 80) //tier 1 cheese
                    {
                        ingredients.Add(cheeseIngredients[0]);
                    }

                    if (randCheese > 80 && randCheese <= 99) //tier 2 cheese
                    {
                        ingredients.Add(cheeseIngredients[1]);
                    }
                    break;
                case 3:
                    randCheese = UnityEngine.Random.Range(0, 100);

                    if (randCheese <= 25) // no cheese
                    {
                        //no cheese
                    }

                    if (randCheese > 25 && randCheese <= 60) //tier 1 cheese
                    {
                        ingredients.Add(cheeseIngredients[0]);
                    }

                    if (randCheese > 60 && randCheese <= 85) //tier 2 cheese
                    {
                        ingredients.Add(cheeseIngredients[1]);
                    }

                    if (randCheese > 85 && randCheese <= 99) //tier 3 cheese
                    {
                        ingredients.Add(cheeseIngredients[2]);
                    }
                    break;
            }

        }

        return ingredients;
    }

    private List<PizzaIngredient> PickTopping(List<PizzaIngredient> ingredientList)
    {
        if(ingredientList.Count == 0)
        {
            dont = true;
            return ingredientList;
        }

        int rand = UnityEngine.Random.Range(0, ingredientList.Count);

        pickedTopping = ingredientList[rand];

        ingredientList.Remove(pickedTopping);

        return ingredientList;
    }

    private int PickToppingAmount(int totalToppings, int i)
    {
        int result = 0;

        //if (dont == false)
        //{
        //    int i = (totalToppings / 2) + (totalToppings % 2);

        //    switch (tier)
        //    {
        //        case 1:
        //            result = UnityEngine.Random.Range(1, (ingredientMax[i - 1].tierOne + 1));
        //            break;
        //        case 2:
        //            result = UnityEngine.Random.Range(1, (ingredientMax[i - 1].tierTwo + 1));
        //            break;
        //        case 3:
        //            result = UnityEngine.Random.Range(1, (ingredientMax[i - 1].tierThree + 1));
        //            break;
        //    }
        //}

        if (differentToppings - i == 1)
        {
            result = 0;
        }

        else
        {
            result = UnityEngine.Random.Range(1, (totalToppings / 2));
        }
        
        return result;
    }

    /// <summary>
    /// Generates random ingredients from list of all ingredients. Uses the ingredients per order for number of ingredients to generate. 
    /// </summary>
    /// <returns>Returns list of ingredients for order for the number of ingredients per order</returns>
    private List<PizzaIngredient> RandomOrderIngredients()
    {
        //int randNumOfIngredients = UnityEngine.Random.Range(minIngredientsPerOrder, maxIngredientsPerOrder + 1);
        List<PizzaIngredient> ingredients = new List<PizzaIngredient>();
        
        //for (int i = 0; i < ingredients.Capacity; ++i)
        //{
        //    ingredients.Add(allPizzaIngredients[UnityEngine.Random.Range(0, allPizzaIngredients.Count)]);
        //}

        return ingredients;
    }
}
