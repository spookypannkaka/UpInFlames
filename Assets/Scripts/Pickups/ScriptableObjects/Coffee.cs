using UnityEngine;

[CreateAssetMenu(fileName = "Coffee", menuName = "Pickup/Behaviors/Coffee")]
public class Coffee : PickupBehavior
{
    public float speedBoost = 2f;

    public override bool UsePickup(GameObject player)
    {
        Debug.Log("USED COFFEE");
        return true;
    }

    public override void ThrowPickup()
    {
        Debug.Log("Coffee discarded.");
    }
}
