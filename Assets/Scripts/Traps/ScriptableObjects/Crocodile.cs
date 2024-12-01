using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crocodile", menuName = "Traps/Behaviors/Crocodile")]
public class Crocodile : TrapBehavior
{
    public override bool UseTrap(GameObject player)
    {
        // play crocodile animation
        // timeout
        player.GetComponent<PlayerController>().Die();
        return true;
    }
}
