using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public IPickup HeldPickup { get; private set; }

    public void PickupItem(GameObject item)
    {
        if (HeldPickup == null)
        {
            IPickup pickup = item.GetComponent<IPickup>();
            if (pickup != null)
            {
                HeldPickup = pickup;

                Destroy(item);
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
                DiscardItem();
            }
        }
    }

    public void DiscardItem()
    {
        if (HeldPickup != null)
        {
            HeldPickup = null;
        }
    }
}
