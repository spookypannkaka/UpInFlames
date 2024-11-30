using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private Vector2 movement;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        playerMovement = GetComponent<PlayerMovement>();
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
    }
}
