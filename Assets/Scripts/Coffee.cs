using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : IPickup
{
    public bool UsePickup(GameObject player)
    {
        return false;
    }

    public void ThrowPickup()
    {

    }
}
