/*
 * Normal Pizza for Normal People
 * IM 389
 * OvenUpgrade
 * Sydney 
 * Sets up oven when the upgraded has been given. Turns on object and reactivates it
 */

using UnityEngine;

public class OvenUpgrade : ItemUpgrades
{
    private OvenBehaviour ob;
    private BoxCollider bc;

    public override void TurnOnUpgrade()
    {
        ob = gameObject.transform.GetChild(0).GetComponent<OvenBehaviour>();
        bc = gameObject.transform.GetChild(0).GetComponent<BoxCollider>();

        ob.enabled = !ob.enabled;
        bc.enabled = !bc.enabled;
    }
}
