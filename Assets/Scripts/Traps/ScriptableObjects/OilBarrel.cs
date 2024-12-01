using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OilBarrel", menuName = "Traps/Behaviors/OilBarrel")]
public class OilBarrel : TrapBehavior
{
    public override bool UseTrap(GameObject player)
    {
        Debug.Log("USED OilBarrel");
        return true;
    }
}
