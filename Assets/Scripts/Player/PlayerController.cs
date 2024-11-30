using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private Vector2 movement;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        playerMovement = GetComponent<PlayerMovement>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        playerMovement.Move(movement);
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        if (playerControls.Interaction.Primary.triggered) {
            /*if (!playerInventory.heldPickup) {
                playerInventory.PickupItem();
            } else {
                // maybe check if you can pick up something
                playerInventory.UseItem();
            }*/
        }

        if (playerControls.Interaction.Secondary.triggered) {
            /*if (playerInventory.heldPickup) {
                playerInventory.DiscardItem();
            }*/
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a pickup trigger
        if (other.CompareTag("Pickup")) // Assuming the trigger is tagged as "Pickup"
        {
            PickupObject pickup = other.GetComponentInParent<PickupObject>();
            if (pickup != null)
            {
                nearbyPickup = pickup; // Store the nearby pickup reference
                Debug.Log($"Entered pickup range of: {pickup.itemName}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the reference when leaving the trigger zone
        if (other.CompareTag("Pickup"))
        {
            PickupObject pickup = other.GetComponentInParent<PickupObject>();
            if (pickup != null && pickup == nearbyPickup)
            {
                nearbyPickup = null;
                Debug.Log("Left pickup range");
            }
        }
    }*/

}
