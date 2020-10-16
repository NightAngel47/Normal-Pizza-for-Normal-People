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
    //[SerializeField] private int minIngredientsPerOrder = 1;
    //[SerializeField] private int maxIngredientsPerOrder = 3;
    [SerializeField] private int minTotalToppingsPerOrder = 0;
    [SerializeField] private int maxTotalToppingsPerOrder = 0;
    [SerializeField] private int rangeLevel = 0; //1,2,3 low,mid,high
    [SerializeField] private List<TotalToppingCurves> toppingTotalRanges = new List<TotalToppingCurves>();
    [SerializeField] private List<MaxIngredientAmount> ingredientMax = new List<MaxIngredientAmount>();
    private PizzaIngredient pickedTopping;
    private bool dont = false;

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

        for (int i = 0; i < forLoopRuns; ++i) //Potential max amount of different ingredients i.e. 6 toppings 6 different options
        {
            dont = false;

            switch (i)
            {
                case 0:
                    tierRand = UnityEngine.Random.Range(0, 100);
                    
                    if (tierRand < 30) //tier 1
                    {
                        oneTemp = PickTopping(oneTemp); //which tier one topping
                        amount = PickToppingAmount(1, totalToppingsPerPizza); //how many of that tier one topping
                    }

                    if(tierRand >= 30 && tierRand < 60) //tier 2
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(2, totalToppingsPerPizza);
                    }

                    if(tierRand >= 60 && tierRand <= 99) //tier 3
                    {
                        threeTemp = PickTopping(threeTemp);
                        amount = PickToppingAmount(3, totalToppingsPerPizza);
                    }
                    
                    break;
                case 1:
                    tierRand = UnityEngine.Random.Range(0, 100);

                    if (tierRand < 30)
                    {
                        oneTemp = PickTopping(oneTemp);
                        amount = PickToppingAmount(1, totalToppingsPerPizza);
                    }

                    if (tierRand >= 30 && tierRand < 60)
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(2, totalToppingsPerPizza);
                    }

                    if (tierRand >= 60 && tierRand <= 99)
                    {
                        threeTemp = PickTopping(threeTemp);
                        amount = PickToppingAmount(3, totalToppingsPerPizza);
                    }

                    break;
                case 2:
                    tierRand = UnityEngine.Random.Range(0, 100);

                    if (tierRand < 30)
                    {
                        oneTemp = PickTopping(oneTemp);
                        amount = PickToppingAmount(1, totalToppingsPerPizza);
                    }

                    if (tierRand >= 30 && tierRand < 60)
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(2, totalToppingsPerPizza);
                    }

                    if (tierRand >= 60 && tierRand <= 99)
                    {
                        threeTemp = PickTopping(threeTemp);
                        amount = PickToppingAmount(3, totalToppingsPerPizza);
                    }

                    break;
                case 3:
                    tierRand = UnityEngine.Random.Range(0, 100);

                    if (tierRand < 30)
                    {
                        oneTemp = PickTopping(oneTemp);
                        amount = PickToppingAmount(1, totalToppingsPerPizza);
                    }

                    if (tierRand >= 30 && tierRand < 60)
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(2, totalToppingsPerPizza);
                    }

                    if (tierRand >= 60 && tierRand <= 99)
                    {
                        threeTemp = PickTopping(threeTemp);
                        amount = PickToppingAmount(3, totalToppingsPerPizza);
                    }

                    break;
                case 4:
                    tierRand = UnityEngine.Random.Range(0, 100);

                    if (tierRand < 30)
                    {
                        oneTemp = PickTopping(oneTemp);
                        amount = PickToppingAmount(1, totalToppingsPerPizza);
                    }

                    if (tierRand >= 30 && tierRand < 60)
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(2, totalToppingsPerPizza);
                    }

                    if (tierRand >= 60 && tierRand <= 99)
                    {
                        threeTemp = PickTopping(threeTemp);
                        amount = PickToppingAmount(3, totalToppingsPerPizza);
                    }

                    break;
                case 5:
                    tierRand = UnityEngine.Random.Range(0, 100);

                    if (tierRand < 30)
                    {
                        oneTemp = PickTopping(oneTemp);
                        amount = PickToppingAmount(1, totalToppingsPerPizza);
                    }

                    if (tierRand >= 30 && tierRand < 60)
                    {
                        twoTemp = PickTopping(twoTemp);
                        amount = PickToppingAmount(2, totalToppingsPerPizza);
                    }

                    if (tierRand >= 60 && tierRand <= 99)
                    {
                        threeTemp = PickTopping(threeTemp);
                        amount = PickToppingAmount(3, totalToppingsPerPizza);
                    }

                    break;
            }

            if (totalToppingTemp - amount < 0) //the amount of toppings to be added is greater than what is available on the pizza
            {
                amount = totalToppingTemp;
                totalToppingTemp -= amount;
            }
            
            else
            {
                totalToppingTemp -= amount;
            }

            if (dont == false)
            {
                for (int k = 0; k < amount; k++)
                {
                    ingredients.Add(pickedTopping);
                }
            }

            if (totalToppingTemp == 0)
            {
                return ingredients;
            }

            if(i == forLoopRuns && totalToppingTemp != 0)
            {
                forLoopRuns = 0;
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

    private int PickToppingAmount(int tier, int totalToppings)
    {
        int i = (totalToppings / 2) + (totalToppings % 2);
        int result = 0;
        Debug.Log(i);

        switch (tier)
        {
            case 1:
                result = UnityEngine.Random.Range(1, (ingredientMax[i - 1].tierOne + 1));
                break;
            case 2:
                result = UnityEngine.Random.Range(1, (ingredientMax[i - 1].tierTwo + 1));
                break;
            case 3:
                result = UnityEngine.Random.Range(1, (ingredientMax[i - 1].tierThree + 1));
                break;
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
