using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crocodile", menuName = "Traps/Behaviors/Crocodile")]
public class Crocodile : TrapBehavior
{
    public override bool UseTrap(GameObject player, TrapObject trap)
    {
        // play crocodile animation
        // timeout
        trap.TriggerAnimation("Snap");
        player.GetComponent<PlayerController>().Die();
        return true;
    }
}
