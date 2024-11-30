using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    public bool UsePickup(GameObject player);

    public void ThrowPickup();
}
