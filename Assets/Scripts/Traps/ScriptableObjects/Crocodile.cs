using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crocodile", menuName = "Traps/Behaviors/Crocodile")]
public class Crocodile : TrapBehavior
{
    public override bool UseTrap(GameObject player)
    {
        Debug.Log("USED CROCODILE");
        return true;
    }
}
