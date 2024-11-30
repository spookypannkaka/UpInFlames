using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private IPickup heldPickup;
    private GameObject heldItemObject;

    public void PickupItem(GameObject item)
    {
        if (heldPickup == null)
        {
            IPickup pickup = item.GetComponent<IPickup>();
            if (pickup != null)
            {
                heldPickup = pickup;
                heldItemObject = item;
                Destroy(item);
                Debug.Log($"Picked up: {item.name}");
            }
        }
    }

    public void UseItem()
    {
        if (heldPickup != null && heldItemObject != null)
        {
            Debug.Log($"Attempting to use: {heldItemObject.name}");
            bool usedSuccessfully = heldPickup.UsePickup(gameObject);

            if (usedSuccessfully)
            {
                Debug.Log($"Used: {heldItemObject.name}");
                DiscardItem();
            }
            else
            {
                Debug.Log($"{heldItemObject.name} could not be used.");
            }
        }
    }

    public void DiscardItem()
    {
        if (heldPickup != null && heldItemObject != null)
        {
            Debug.Log($"Discarded: {heldItemObject.name}");
            heldPickup = null;
            heldItemObject = null;
        }
    }
}
