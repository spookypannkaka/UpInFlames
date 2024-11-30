using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 movementInput) {
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y) * moveSpeed;
        Vector3 newPosition = rb.position + movement * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}
