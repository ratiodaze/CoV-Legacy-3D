using UnityEngine;
using Sirenix.OdinInspector;

// Coordinates movement modules and exposes movement state
public class MovementCoordinator : SerializedMonoBehaviour
{
    // ──────────────────────────────────────────────────────────────
    // 📦 Core Movement Modules
    // ──────────────────────────────────────────────────────────────

    [BoxGroup("Modules"), SerializeField]
    private MovementModule movementModule;

    [BoxGroup("Modules"), SerializeField]
    private DashSprintHandler dashHandler;

    // Jump, climb, and airborne handling are omitted for the 2D variant

    // ──────────────────────────────────────────────────────────────
    // 📌 State Tracking (exposed to Animation / Combat)
    // ──────────────────────────────────────────────────────────────

    [ShowInInspector, ReadOnly]
    public bool IsSprinting => dashHandler?.IsSprinting ?? false;

    [ShowInInspector, ReadOnly]
    public bool IsDashing => dashHandler?.IsDashing ?? false;

    [ShowInInspector, ReadOnly]
    public Vector2 MoveDirection => movementModule?.CurrentMoveDirection ?? Vector2.zero;

    [ShowInInspector, ReadOnly]
    public float CurrentSpeed => movementModule?.CurrentSpeed ?? 0f;

    // ──────────────────────────────────────────────────────────────
    // 🔁 Unity Lifecycle Hooks
    // ──────────────────────────────────────────────────────────────

    private void Update()
    {
        movementModule?.Tick(Time.deltaTime);
        dashHandler?.Tick(Time.deltaTime);
    }

    // Optional FixedUpdate or LateUpdate if needed later

    // ──────────────────────────────────────────────────────────────
    // ✨ Public Methods
    // ──────────────────────────────────────────────────────────────

    public void LockMovement() => movementModule?.LockMovement();
    public void UnlockMovement() => movementModule?.UnlockMovement();

    public void TriggerDash() => dashHandler?.TryDash();
  


    public void SetInputDirection(Vector2 input)
    {
        movementModule?.SetMovementInput(input);
        dashHandler?.SetInputDirection(input);
    }

    public void SetMovementMultiplier(float multiplier)
    {
        movementModule?.SetSpeedMultiplier(multiplier);
    }
}
