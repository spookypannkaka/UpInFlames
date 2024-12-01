using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public IPickup HeldPickup { get; private set; }
    public Transform modelDisplayAnchor;
    private GameObject displayedModel;

    [Header("Rotation Settings")]
    public float rotationSpeed = 20f;
    public float tiltAngle = 10f;
    [Header("UI Settings")]
    public TMP_Text instructionText;

    private void Start() {
        instructionText.text = "";
    }

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

    public void UpdateInstructionText(PickupObject closestPickup, string primaryKey, string secondaryKey)
    {
        if (HeldPickup != null)
        {
            // Show "Use" and "Throw" instructions
            instructionText.text = $"[{primaryKey}] Use {HeldPickup.Name}\n[{secondaryKey}] Throw {HeldPickup.Name}";
        }
        else if (closestPickup != null)
        {
            // Show "Pick up" instruction
            instructionText.text = $"[{primaryKey}] Pick up {closestPickup.Name}";
        }
        else
        {
            // Clear the text if no interactions are available
            instructionText.text = "";
        }
    }
}
