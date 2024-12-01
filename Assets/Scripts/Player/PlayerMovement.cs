using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How fast the player is moving.")]
    public float moveSpeed = 5f;
    [Tooltip("How much the player slows down.")]
    public float drag = 2f;
    [Tooltip("The max speed the player can reach.")]
    public float maxSpeed = 10f;

    [Header("Rotation Settings")]
    [Tooltip("How fast the player rotates towards movement direction.")]
    public float rotationSpeed = 10f;
    [Tooltip("How much the player tilts while moving.")]
    public float tiltIntensity = 10f;
    [Tooltip("Speed at which the player corrects to upright.")]
    public float correctionSpeed = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 movementInput) {
        Vector3 movementForce = new Vector3(movementInput.x, 0f, movementInput.y) * moveSpeed;
        rb.AddForce(movementForce, ForceMode.Force);

        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + new Vector3(0f, rb.velocity.y, 0f);
        }
        if (horizontalVelocity.sqrMagnitude > 0.01f)
        {
            // Calculate the target rotation based on velocity direction
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity, Vector3.up);

            // Smoothly rotate towards the target direction
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        rb.velocity = new Vector3(rb.velocity.x * (1f - drag * Time.fixedDeltaTime),
                                  rb.velocity.y,
                                  rb.velocity.z * (1f - drag * Time.fixedDeltaTime));

        HandleTilt(movementInput);
    }

    private void HandleTilt(Vector2 movementInput)
    {
        if (movementInput.sqrMagnitude > 0.01f)
        {
            float tiltAngleX = -movementInput.y * tiltIntensity;
            float tiltAngleZ = movementInput.x * tiltIntensity;

            Quaternion tiltRotation = Quaternion.Euler(tiltAngleX, rb.rotation.eulerAngles.y, tiltAngleZ);
            rb.MoveRotation(tiltRotation);
        }
        else
        {
            // Gradually correct the rotation to upright when there's no input
            Quaternion uprightRotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, uprightRotation, correctionSpeed * Time.fixedDeltaTime));
        }
    }
}
