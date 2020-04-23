using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemUpgrades : MonoBehaviour
{

    //displays items with the available for purchase material during ugrade period
    public void ShowItem()
    {
        gameObject.SetActive(true);
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
        gameObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject.GetComponent<ItemUpgrades>()); // Destroy item upgrade so it is not readded to upgrade system

    }

    //toggles object functionality
    public abstract void TurnOnUpgrade();
}
