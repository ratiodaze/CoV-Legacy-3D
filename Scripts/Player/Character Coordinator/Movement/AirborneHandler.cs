using UnityEngine;
using Sirenix.OdinInspector;

// Tracks airborne state (falling, knockback, launched)
public class AirborneHandler : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundedCheckDistance = 0.2f;
    [SerializeField] private ClimbHandler climbHandler;

    [Header("Airborne Settings")]
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float terminalVelocity = -40f;

    [Header("State")]
    [ShowInInspector, ReadOnly] private bool isGrounded;
    [ShowInInspector, ReadOnly] private Vector3 verticalVelocity;

    public bool IsGrounded => isGrounded;
    public bool IsAirborne => !isGrounded;
    public Vector3 VerticalVelocity => verticalVelocity;
    public float Gravity => gravity;

    public event System.Action OnLanded;

    // ──────────────────────────────────────────────────────────────
    // 🔁 Tick
    // ──────────────────────────────────────────────────────────────

    public void Tick(float deltaTime)
    {
        // Suppress gravity while climbing
        if (climbHandler != null && climbHandler.IsClimbing)
            return;

        UpdateGroundedState();
        ApplyGravity(deltaTime);
        MoveVertical(deltaTime);
    }

    private void UpdateGroundedState()
    {
        if (climbHandler != null && climbHandler.IsClimbing)
        {
            isGrounded = false;
            return;
        }

        Vector3 origin = controller.bounds.center;
        float rayLength = (controller.height / 2f) + groundedCheckDistance;

        bool wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(origin, Vector3.down, rayLength, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            OnLanded?.Invoke();
        }

        if (isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -0.05f;
        }
    }

    private void ApplyGravity(float deltaTime)
    {
        if (!isGrounded)
        {
            verticalVelocity.y += gravity * deltaTime;
            verticalVelocity.y = Mathf.Max(verticalVelocity.y, terminalVelocity);
        }
    }

    private void MoveVertical(float deltaTime)
    {
        if (verticalVelocity.y != 0f)
        {
            controller.Move(verticalVelocity * deltaTime);
        }
    }

    // ──────────────────────────────────────────────────────────────
    // 🚀 External Forces
    // ──────────────────────────────────────────────────────────────

    public void Launch(Vector3 force)
    {
        verticalVelocity = force;
        isGrounded = false;
    }

    public void ForceJump(float upwardVelocity)
    {
        verticalVelocity.y = upwardVelocity;
        isGrounded = false;
    }
}
