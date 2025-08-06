using UnityEngine;
using Sirenix.OdinInspector;

// Handles dash burst and sprint state
public class DashSprintHandler : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementModule movementModule;
    [SerializeField] private AirborneHandler airborneHandler;

    [BoxGroup("Dash Settings")]
    [SerializeField] private float dashSpeedMultiplier = 2.2f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashCooldown = 0.6f;

    [BoxGroup("Sprint Settings")]
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float sprintStopDelay = 0.2f; // Grace period after movement stops

    [BoxGroup("State"), ShowInInspector, ReadOnly]
    private bool isDashing;
    [BoxGroup("State"), ShowInInspector, ReadOnly]
    private bool isSprinting;

    private float dashTimer;
    private float cooldownTimer;
    private float sprintStopTimer;
    private Vector2 inputDirection;

    public bool IsDashing => isDashing;
    public bool IsSprinting => isSprinting;

    // ──────────────────────────────────────────────────────────────
    // 🔁 Tick
    // ──────────────────────────────────────────────────────────────

    public void Tick(float deltaTime)
    {
        HandleCooldown(deltaTime);
        HandleDashState(deltaTime);
        HandleSprintState(deltaTime);
    }

    private void HandleCooldown(float deltaTime)
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= deltaTime;
    }

    private void HandleDashState(float deltaTime)
    {
        if (isDashing)
        {
            dashTimer -= deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                isSprinting = true;
                sprintStopTimer = sprintStopDelay;
                movementModule.SetSpeedMultiplier(sprintSpeedMultiplier);
            }
        }
    }

    private void HandleSprintState(float deltaTime)
    {
        if (!isDashing && isSprinting)
        {
            if (inputDirection.magnitude < 0.1f)
            {
                sprintStopTimer -= deltaTime;
                if (sprintStopTimer <= 0f)
                {
                    isSprinting = false;
                    movementModule.SetSpeedMultiplier(1f);
                }
            }
            else
            {
                sprintStopTimer = sprintStopDelay; // Reset delay if moving
            }
        }
    }

    // ──────────────────────────────────────────────────────────────
    // 🎮 Input
    // ──────────────────────────────────────────────────────────────

    public void SetInputDirection(Vector2 input)
    {
        inputDirection = input;
    }

    public void TryDash()
    {
        if (cooldownTimer > 0f || movementModule == null || airborneHandler == null || !airborneHandler.IsGrounded)
            return;

        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
        movementModule.SetSpeedMultiplier(dashSpeedMultiplier);
    }
}
