using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private Vector2 movement;
    private PickupObject nearbyPickup;
    public int score;

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
        if(playerControls != null)
        {
            playerControls.Disable();
        }
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
            if (playerInventory.HeldPickup == null && nearbyPickup != null) {
                playerInventory.PickupItem(nearbyPickup.gameObject);
                nearbyPickup = null;
            } else {
                playerInventory.UseItem();
            }
        }

        if (playerControls.Interaction.Secondary.triggered) {
            if (playerInventory.HeldPickup != null) {
                playerInventory.DiscardItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupObject pickup = other.GetComponentInParent<PickupObject>();
        if (pickup != null)
        {
            nearbyPickup = pickup;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupObject pickup = other.GetComponentInParent<PickupObject>();
        if (pickup != null && pickup == nearbyPickup)
        {
            nearbyPickup = null;
        }
    }

    public void ClimbUpStairs()
    {
        score++;
        transform.position = new Vector3(0, 1, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
