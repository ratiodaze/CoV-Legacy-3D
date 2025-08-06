using UnityEngine;
using Sirenix.OdinInspector;

// Handles jump permission logic and delegates vertical motion
public class JumpHandler : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private AirborneHandler airborneHandler;
    [SerializeField] private ClimbHandler climbHandler;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2.0f;

    [Header("State")]
    [ShowInInspector, ReadOnly] private bool jumpLocked;

    // ──────────────────────────────────────────────────────────────
    // 🧠 Unity Lifecycle
    // ──────────────────────────────────────────────────────────────

    private void Awake()
    {
        if (airborneHandler != null)
            airborneHandler.OnLanded += UnlockJump;
    }

    private void OnDisable()
    {
        if (airborneHandler != null)
            airborneHandler.OnLanded -= UnlockJump;
    }

    // ──────────────────────────────────────────────────────────────
    // ⬆️ Jump Trigger
    // ──────────────────────────────────────────────────────────────

    public void TryJump()
    {
        // Detach from climb if climbing
        if (climbHandler != null && climbHandler.IsClimbing)
        {
            climbHandler.ExitClimb();
            return;
        }

        if (!airborneHandler.IsGrounded || jumpLocked)
            return;

        float gravity = airborneHandler.Gravity;
        float jumpVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);

        airborneHandler.ForceJump(jumpVelocity);
        jumpLocked = true;
    }

    public void UnlockJump()
    {
        jumpLocked = false;
    }
}
