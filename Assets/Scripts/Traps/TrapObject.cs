using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour, ITrap
{
    public TrapBehavior behavior;

    private void Start()
    {
        // Dynamically assign behavior if none is set
        if (behavior == null)
        {
            AssignBehaviorFromPrefabName();
        }
    }

    private void AssignBehaviorFromPrefabName()
    {
        // Use the prefab's name to fetch a matching behavior
        string prefabName = gameObject.name.Replace("(Clone)", "").Trim(); // Remove "(Clone)" from instantiated prefab names
        behavior = TrapBehaviorManager.GetBehaviorByName(prefabName);

        if (behavior != null)
        {
            Debug.Log($"Assigned behavior '{behavior.trapName}' to trap '{prefabName}'.");
        }
        else
        {
            Debug.LogWarning($"No matching behavior found for trap '{prefabName}'.");
        }
    }

    public bool UseTrap(GameObject player)
    {
        if (behavior != null)
        {
            return behavior.UseTrap(player);
        }
        Debug.LogWarning("No behavior assigned to this trap.");
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            UseTrap(collision.gameObject);
        }
    }
}
