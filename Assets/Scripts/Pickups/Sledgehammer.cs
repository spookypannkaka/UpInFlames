using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sledgehammer : MonoBehaviour
{
    public float range = 0.5f;

    public bool UsePickup(GameObject player)
    {
        RaycastHit hit;
        Vector3 direction = player.transform.forward;
        LayerMask layerMask = LayerMask.GetMask("Wall");

        if (Physics.Raycast(player.transform.position, direction, out hit, range, layerMask))
        {
            Destroy(hit.transform.gameObject);
            return true; // Used pickup successfully
        }

        return false; // Used pickup unsuccessfully, should not be "consumed"
    }

    public void ThrowPickup()
    { 
    
    }
}
