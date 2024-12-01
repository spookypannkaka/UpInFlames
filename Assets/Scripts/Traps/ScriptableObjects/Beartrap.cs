using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Beartrap", menuName = "Traps/Behaviors/Beartrap")]
public class Beartrap : TrapBehavior
{
    private bool isActive = true;

    public override bool UseTrap(GameObject player, TrapObject trap)
    {
        if (trap == null) return false;

        trap.TriggerAnimation("Snap");

        player.GetComponent<PlayerController>().StunPlayer(1.0f);
        return true;
    }
}
