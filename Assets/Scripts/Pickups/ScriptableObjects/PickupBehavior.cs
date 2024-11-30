using UnityEngine;

[CreateAssetMenu(fileName = "NewPickupBehavior", menuName = "Pickup/Behavior")]
public class PickupBehavior : ScriptableObject
{
    public string pickupName;
    public GameObject associatedModel;

    public virtual bool UsePickup(GameObject player)
    {
        Debug.Log($"{pickupName} used.");
        return true;
    }

    public virtual void ThrowPickup()
    {
        Debug.Log($"{pickupName} thrown.");
    }
}
