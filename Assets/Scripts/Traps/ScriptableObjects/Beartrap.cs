using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Beartrap", menuName = "Traps/Behaviors/Beartrap")]
public class Beartrap : TrapBehavior
{
    public override bool UseTrap(GameObject player)
    {
        player.GetComponent<PlayerController>().StunPlayer(1.0f);
        return true;
    }
}
