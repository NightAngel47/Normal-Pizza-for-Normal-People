using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemUpgrades : MonoBehaviour
{
    public Material blueprintMaterial; //material for when it is not avaiable for purchase
    public Material activeMaterial; //material for when it is purchased its 'normal' material

    private MoneyTracker mt; //reference to moneytracker so that it can purchase the upgrades and subtract from total money amount

    public int itemCost; //cost of upgrade. set individually by each upgrade

    public OrderCreation oc;
    public bool isTopping;

    private void Start()
    {
        //finds money tracker ref
        mt = FindObjectOfType<MoneyTracker>();
        oc = FindObjectOfType<OrderCreation>();
    }

    //displays items with the available for purchase material during ugrade period
    public void ShowItem()
    {
        gameObject.SetActive(true);
        ChangeMaterial(blueprintMaterial);
    }

    //hides items when upgrade period is done
    public void HideItem()
    {
        gameObject.SetActive(false); ;
    }

    //When item is being purchased/when item is now avialble to player via forced upgrade
    public void Purchase()
    {
        Debug.Log("purch");
        gameObject.SetActive(true); //make sure it is active, probably do not need this line
        TurnOnUpgrade(); //makes sure upgrade funcitonality works 
        FindObjectOfType<UpgradeSystem>().RemovedPurchasedUpgrade(gameObject); // Remove this gameobject from upgrade system
        if(isTopping == true)
        {
            oc.allPizzaIngredients.Add(gameObject.GetComponent<PizzaIngredientSpawner>().pizzaIngredientToSpawn.GetComponent<IngredientHitEffect>().
                spawnObjectOnCollision.GetComponent<PizzaIngredient>());
        }
        Destroy(gameObject.GetComponent<ItemUpgrades>()); // Destroy item upgrade so it is not readded to upgrade system

        //PLAYER CHOOSE UPGRADE LEFTOVER CODE
        //if (mt.Purchase(itemCost)) //do we have enough money for the upgrade
        //{
        //    gameObject.SetActive(true); //make sure it is active, probably do not need this line
        //    TurnOffPurchaseCollider(); //turns off purchase collider(which is what detects when the player wants to buy) and its timer as well
        //    TurnOnUpgrade(); //makes sure upgrade funcitonality works 
        //    ChangeMaterial(activeMaterial); //changes it to the active normal material
        //    FindObjectOfType<UpgradeSystem>().RemovedPurchasedUpgrade(gameObject); // Remove this gameobject from upgrade system
        //    Destroy(gameObject.GetComponent<ItemUpgrades>()); // Destroy item upgrade so it is not readded to upgrade system
        //}

        //else //not enonugh money
        //{
        //    Debug.Log("Cannot Buy Not Enough Mons");
        //}
    }

    //toggles object functionality
    public abstract void TurnOnUpgrade();

    //Turns off the purchase collider and the timer for confirming the purchase
    protected abstract void TurnOffPurchaseCollider();

    //updates the material of the upgrade
    protected abstract void ChangeMaterial(Material changeMat);
}
