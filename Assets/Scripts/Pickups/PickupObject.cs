using UnityEngine;

public class PickupObject : MonoBehaviour, IPickup
{
    public string itemName = "Placeholder Item";

    public bool UsePickup(GameObject player)
    {
        Debug.Log($"{itemName} has been used by the player!");
        return true;
    }

    public void ThrowPickup()
    {
        Debug.Log($"{itemName} was thrown away!");
    }
}
