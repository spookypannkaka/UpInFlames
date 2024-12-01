using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sledgehammer", menuName = "Pickup/Behaviors/Sledgehammer")]
public class Sledgehammer : PickupBehavior
{
    public float range = 0.5f;

    public override bool UsePickup(GameObject player)
    {
        Vector3 direction = player.transform.forward;
        LayerMask layerMask = LayerMask.GetMask("Wall");

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, 0.5f, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            Destroy(hitCollider.transform.gameObject);
        }

        return true;
    }

    public override void ThrowPickup()
    {
        Debug.Log("Sledgehammer discarded.");
    }
}
