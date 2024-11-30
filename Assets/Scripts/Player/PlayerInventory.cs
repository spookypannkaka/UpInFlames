using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public IPickup HeldPickup { get; private set; }
    public Transform modelDisplayAnchor;
    private GameObject displayedModel;

    [Header("Rotation Settings")]
    public float rotationSpeed = 20f;
    public float tiltAngle = 10f;

    private void Update()
    {
        if (displayedModel != null)
        {
            RotateModel();
        }
    }

    public void PickupItem(GameObject item)
    {
        if (HeldPickup == null)
        {
            IPickup pickup = item.GetComponent<IPickup>();
            if (pickup != null)
            {
                HeldPickup = pickup;

                Destroy(item);

                DisplayModel(pickup as PickupObject);
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
            ClearDisplay();
        }
    }

    private void DisplayModel(PickupObject pickup)
    {
        ClearDisplay();

        if (pickup != null && pickup.behavior != null)
        {
            GameObject modelPrefab = pickup.behavior.associatedModel;
            if (modelPrefab != null)
            {
                // Instantiate the model in front of the ModelCamera
                displayedModel = Instantiate(modelPrefab, modelDisplayAnchor);
                displayedModel.transform.localPosition = modelDisplayAnchor.transform.localPosition;
                displayedModel.transform.localRotation = Quaternion.Euler(0, 0, tiltAngle);

                // Assign the model to the correct layer
                SetLayerRecursively(displayedModel, LayerMask.NameToLayer("UI"));
            }
        }
    }

    private void RotateModel()
    {
        displayedModel.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void ClearDisplay()
    {
        if (displayedModel != null)
        {
            Destroy(displayedModel);
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
