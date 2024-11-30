using UnityEngine;

public class PickupObject : MonoBehaviour, IPickup
{
    public PickupBehavior behavior;
    public Transform modelHolder;

    private void Start()
    {
        AssignRandomBehavior();
    }

    private void AssignRandomBehavior()
    {
        // Get a random behavior from the PickupBehaviorManager
        behavior = PickupBehaviorManager.GetRandomBehavior();

        if (behavior != null)
        {
            // Replace the visual model to match the behavior
            ReplaceModel(behavior.associatedModel);

            Debug.Log($"Assigned behavior: {behavior.pickupName} to {gameObject.name}");
        }
    }

    private void ReplaceModel(GameObject newModelPrefab)
    {
        // Clear any existing model
        foreach (Transform child in modelHolder)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the new model as a child of the modelHolder
        if (newModelPrefab != null)
        {
            Instantiate(newModelPrefab, modelHolder.position, modelHolder.rotation, modelHolder);
        }
    }

    public bool UsePickup(GameObject player)
    {
        if (behavior != null)
        {
            return behavior.UsePickup(player);
        }
        Debug.LogWarning("No behavior assigned to this pickup.");
        return false;
    }

    public void ThrowPickup()
    {
        if (behavior != null)
        {
            behavior.ThrowPickup();
        }
        else
        {
            Debug.LogWarning("No behavior assigned to this pickup.");
        }
    }
}
