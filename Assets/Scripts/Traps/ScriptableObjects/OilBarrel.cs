using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OilBarrel", menuName = "Traps/Behaviors/OilBarrel")]
public class OilBarrel : TrapBehavior
{
    public override bool UseTrap(GameObject player, TrapObject trap)
    {
        Debug.Log("USED OilBarrel");
        return true;
    }
}
