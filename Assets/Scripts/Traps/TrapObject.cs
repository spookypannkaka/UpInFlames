using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour, ITrap
{
    public TrapBehavior behavior;

    public bool UseTrap(GameObject player)
    {
        if (behavior != null)
        {
            return behavior.UseTrap(player);
        }
        Debug.LogWarning("No behavior assigned to this trap.");
        return false;
    }
}
