using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private Vector2 movement;
    private List<PickupObject> pickupsInTrigger = new List<PickupObject>();
    private PickupObject closestPickup;
    public int score;
    private bool isStunned = false;
    private bool isDying = false;

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
        if (isDying) return;
        playerMovement.Move(movement);
    }

    private void PlayerInput() {
        if (isStunned)
        {
            movement = Vector2.zero;
            return;
        }

        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        if (playerControls.Interaction.Primary.triggered) {
            if (playerInventory.HeldPickup == null && closestPickup != null) {
                playerInventory.PickupItem(closestPickup.gameObject);
                pickupsInTrigger.Remove(closestPickup);
                closestPickup.EnableHighlight(false);
                closestPickup = null;
                UpdateClosestPickup();
                playerInventory.UpdateInstructionText(closestPickup, playerControls.Interaction.Primary.GetBindingDisplayString(), playerControls.Interaction.Secondary.GetBindingDisplayString());
            } else {
                playerInventory.UseItem();
                playerInventory.UpdateInstructionText(closestPickup, playerControls.Interaction.Primary.GetBindingDisplayString(), playerControls.Interaction.Secondary.GetBindingDisplayString());
            }
        }

        if (playerControls.Interaction.Secondary.triggered) {
            if (playerInventory.HeldPickup != null) {
                playerInventory.DiscardItem();
                playerInventory.UpdateInstructionText(closestPickup, playerControls.Interaction.Primary.GetBindingDisplayString(), playerControls.Interaction.Secondary.GetBindingDisplayString());
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
            playerInventory.UpdateInstructionText(closestPickup, playerControls.Interaction.Primary.GetBindingDisplayString(), playerControls.Interaction.Secondary.GetBindingDisplayString());
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
            playerInventory.UpdateInstructionText(closestPickup, playerControls.Interaction.Primary.GetBindingDisplayString(), playerControls.Interaction.Secondary.GetBindingDisplayString());
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

    public void Die(float delay = 5.0f)
    {
        if (isDying) return;
        isDying = true;
        playerMovement.enabled = false;
        StartCoroutine(DieCoroutine(delay));
    }

    private IEnumerator DieCoroutine(float delay)
    {
        // Wait for the delay duration
        yield return new WaitForSeconds(delay);

        // Reset the score and reload the scene
        score = 0;
        transform.position = new Vector3(0, 1, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        isDying = false;
    }

    public void StunPlayer(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        // Temporarily disable player movement
        playerMovement.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Freeze the Rigidbody's constraints to prevent external forces
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Wait for the stun duration
        yield return new WaitForSeconds(duration);

        // Re-enable movement and restore Rigidbody's constraints
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        playerMovement.enabled = true;
        isStunned = false;
    }

}
