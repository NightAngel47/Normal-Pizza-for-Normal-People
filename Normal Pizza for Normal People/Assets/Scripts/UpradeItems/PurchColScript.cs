using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PurchColScript : MonoBehaviour
{
    private ItemUpgrades iu;

    private void Start()
    {
        iu = gameObject.GetComponentInParent<ItemUpgrades>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponentInParent<HandCollider>())
        {
            iu.Purchase();
        }
    }
}
