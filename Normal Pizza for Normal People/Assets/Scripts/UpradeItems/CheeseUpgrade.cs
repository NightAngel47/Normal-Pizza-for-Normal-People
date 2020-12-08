using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseUpgrade : ItemUpgrades
{
    public override void TurnOnUpgrade()
    {
        gameObject.SetActive(true);
    }
}
