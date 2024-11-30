using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public IPickup HeldPickup { get; private set; }
    private string heldItemName;

    public void PickupItem(GameObject item)
    {
        if (HeldPickup == null)
        {
            IPickup pickup = item.GetComponent<IPickup>();
            if (pickup != null)
            {
                HeldPickup = pickup;
                heldItemName = item.name;

                Destroy(item);

                Debug.Log($"Picked up: {heldItemName}");
            }
        }
    }

    public void UseItem()
    {
        if (HeldPickup != null)
        {
            bool usedSuccessfully = HeldPickup.UsePickup(gameObject);

            if (usedSuccessfully)
            {
                Debug.Log($"Used: {heldItemName}");
                DiscardItem();
            }
        }
    }

    public void DiscardItem()
    {
        if (HeldPickup != null)
        {
            Debug.Log($"Discarded: {heldItemName}");
            HeldPickup = null;
            heldItemName = null;
        }
    }
}
