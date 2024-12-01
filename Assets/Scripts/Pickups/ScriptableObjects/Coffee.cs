using UnityEngine;

[CreateAssetMenu(fileName = "Coffee", menuName = "Pickup/Behaviors/Coffee")]
public class Coffee : PickupBehavior
{
    public float speedBoost = 2f;
    public float boostDuration = 5f;

    public override bool UsePickup(GameObject player)
    {
        player.GetComponent<PlayerMovement>().ApplySpeedBoost(speedBoost, boostDuration);
        return true;
    }

    public override void ThrowPickup()
    {
        Debug.Log("Coffee discarded.");
    }
}
