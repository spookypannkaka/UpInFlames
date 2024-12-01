using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    string Name { get; }
    public bool UsePickup(GameObject player);

    public void ThrowPickup();
}
