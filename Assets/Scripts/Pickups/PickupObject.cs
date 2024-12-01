using UnityEngine;

public class PickupObject : MonoBehaviour, IPickup
{
    public PickupBehavior behavior;
    public Transform modelHolder;
    public Material glowingMaterial;
    private Material originalMaterial;
    private Renderer objectRenderer;
    private GameObject instantiatedModel;

    public string Name => behavior != null ? behavior.pickupName : "Unknown";

    private void Start()
    {
        AssignRandomBehavior();
    }

    private void AssignRandomBehavior()
    {
        behavior = PickupBehaviorManager.GetRandomBehavior();

        if (behavior != null)
        {
            ReplaceModel(behavior.associatedModel);
        }
    }

    private void ReplaceModel(GameObject newModelPrefab)
    {
        foreach (Transform child in modelHolder)
        {
            Destroy(child.gameObject);
        }

        if (newModelPrefab != null)
        {
            instantiatedModel = Instantiate(newModelPrefab, modelHolder.position, modelHolder.rotation, modelHolder);
            objectRenderer = instantiatedModel.GetComponent<Renderer>();
            if (objectRenderer != null && objectRenderer.materials.Length > 0)
            {
                originalMaterial = objectRenderer.materials[0];
            }
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

    public void EnableHighlight(bool status)
    {
        if (objectRenderer != null && objectRenderer.materials.Length > 0)
        {
            Material[] materials = objectRenderer.materials;
            if (status)
            {
                materials[0] = glowingMaterial;
            }
            else
            {
                materials[0] = originalMaterial;
            }
            objectRenderer.materials = materials;
        }
    }
}
