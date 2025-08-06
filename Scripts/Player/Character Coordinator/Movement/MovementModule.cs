using UnityEngine;
using Sirenix.OdinInspector;

// Core directional locomotion handler
public class MovementModule : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform; // Used for camera-relative movement

    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float rotationSpeed = 720f; // Degrees per second

    [Header("Debug")]
    [ShowInInspector, ReadOnly] private Vector3 currentVelocity = Vector3.zero;
    [ShowInInspector, ReadOnly] private Vector2 moveInput = Vector2.zero;
    [ShowInInspector, ReadOnly] private bool movementLocked = false;
    [ShowInInspector, ReadOnly] private float speedMultiplier = 1f;

    public Vector3 CurrentMoveDirection => currentVelocity.normalized;
    public float CurrentSpeed => currentVelocity.magnitude;
    public float BaseSpeed => baseSpeed;
    public void SetBaseSpeed(float newSpeed) => baseSpeed = newSpeed;

    // ──────────────────────────────────────────────────────────────
    // 🔁 Tick
    // ──────────────────────────────────────────────────────────────

    public void Tick(float deltaTime)
    {
        if (movementLocked)
        {
            currentVelocity = Vector3.zero;
            return;
        }

        // Calculate camera-relative movement direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputWorld = camForward * moveInput.y + camRight * moveInput.x;
        inputWorld.Normalize();

        Vector3 targetVelocity = inputWorld * baseSpeed * speedMultiplier;
        float blend = targetVelocity.magnitude > 0.1f ? acceleration : deceleration;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, blend * deltaTime);

        controller.Move(currentVelocity * deltaTime);

        // Smooth rotation towards movement direction
        if (inputWorld.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputWorld);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * deltaTime);
        }
    }

    // ──────────────────────────────────────────────────────────────
    // 🎮 Input / External Control
    // ──────────────────────────────────────────────────────────────

    public void SetMovementInput(Vector2 input)
    {
        moveInput = input;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void LockMovement() => movementLocked = true;
    public void UnlockMovement() => movementLocked = false;
}
