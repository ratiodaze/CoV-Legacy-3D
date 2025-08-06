using UnityEngine;
using Sirenix.OdinInspector;

// Core directional locomotion handler
public class MovementModule : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D body;

    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float rotationSpeed = 720f; // Degrees per second

    [Header("Debug")]
    [ShowInInspector, ReadOnly] private Vector2 currentVelocity = Vector2.zero;
    [ShowInInspector, ReadOnly] private Vector2 moveInput = Vector2.zero;
    [ShowInInspector, ReadOnly] private bool movementLocked = false;
    [ShowInInspector, ReadOnly] private float speedMultiplier = 1f;

    public Vector2 CurrentMoveDirection => currentVelocity.normalized;
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
            currentVelocity = Vector2.zero;
            body.velocity = Vector2.zero;
            return;
        }

        Vector2 targetVelocity = moveInput * baseSpeed * speedMultiplier;
        float blend = targetVelocity.magnitude > 0.1f ? acceleration : deceleration;
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, blend * deltaTime);

        body.MovePosition(body.position + currentVelocity * deltaTime);

        if (currentVelocity.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
            float angle = Mathf.MoveTowardsAngle(body.rotation, targetAngle, rotationSpeed * deltaTime);
            body.MoveRotation(angle);
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
