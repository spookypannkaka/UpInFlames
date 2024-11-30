using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private Vector2 movement;
    private List<PickupObject> pickupsInTrigger = new List<PickupObject>();
    private PickupObject closestPickup;
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
            if (playerInventory.HeldPickup == null && closestPickup != null) {
                playerInventory.PickupItem(closestPickup.gameObject);
                pickupsInTrigger.Remove(closestPickup);
                closestPickup.EnableHighlight(false);
                closestPickup = null;
                UpdateClosestPickup();
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
            pickupsInTrigger.Add(pickup);
            UpdateClosestPickup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupObject pickup = other.GetComponentInParent<PickupObject>();
        if (pickup != null && pickupsInTrigger.Contains(pickup))
        {
            pickupsInTrigger.Remove(pickup);
            if (pickup == closestPickup)
            {
                closestPickup.EnableHighlight(false);
                closestPickup = null;
            }
            UpdateClosestPickup();
        }
    }

    private void UpdateClosestPickup()
    {
        if (pickupsInTrigger.Count == 0 || playerInventory.HeldPickup != null)
        {
            if (closestPickup != null)
            {
                closestPickup.EnableHighlight(false);
                closestPickup = null;
            }
            return;
        }

        PickupObject newClosestPickup = pickupsInTrigger
            .OrderBy(p => Vector3.Distance(transform.position, p.transform.position))
            .FirstOrDefault();

        if (newClosestPickup != closestPickup)
        {
            if (closestPickup != null)
            {
                closestPickup.EnableHighlight(false);
            }

            closestPickup = newClosestPickup;
            if (closestPickup != null)
            {
                closestPickup.EnableHighlight(true);
            }
        }
    }

    public void ClimbUpStairs()
    {
        score++;
        transform.position = new Vector3(0, 1, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
