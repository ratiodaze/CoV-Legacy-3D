using UnityEngine;
using Sirenix.OdinInspector;

// Handles climbing interactions with ladders, vines, or walls
[RequireComponent(typeof(CharacterController))]
public class ClimbHandler : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private MovementCoordinator movement;
    [SerializeField] private ClimbMovementController climbMovement;
    //[SerializeField] private AnimationCoordinator animation; // Optional future integration

    [Header("Climb Settings")]
    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private float maxClimbableAngle = 60f;
    [SerializeField] private LayerMask climbableLayer;

    [Header("State")]
    [ShowInInspector, ReadOnly] private bool isClimbing;
    [ShowInInspector, ReadOnly] private Vector3 climbNormal = Vector3.zero;
    [ShowInInspector, ReadOnly] private ClimbableSurface currentSurface;
    [ShowInInspector, ReadOnly] private Vector2 moveInput = Vector2.zero;

    public bool IsClimbing => isClimbing;
    public ClimbableSurface CurrentSurface => currentSurface;
    public Vector2 MoveInput => moveInput;

    // ──────────────────────────────────────────────────────────────
    // 📌 Climb Detection
    // ──────────────────────────────────────────────────────────────

    public void TryBeginClimbFromInteract()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, climbableLayer);
        foreach (var hit in hits)
        {
            ClimbableSurface surface = hit.GetComponentInParent<ClimbableSurface>();
            if (surface == null) continue;

            Vector3 toSurface = hit.transform.position - transform.position;
            if (Vector3.Angle(toSurface.normalized, Vector3.up) > maxClimbableAngle)
                continue;

            EnterClimb(surface);
            return;
        }
    }

    public void TryBeginClimbFromJump(ClimbableSurface surface)
    {
        // This would be called when mid-air raycast hits a climbable
        EnterClimb(surface);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClimbing) return;

        ClimbableSurface surface = other.GetComponentInParent<ClimbableSurface>();
        if (surface != null && surface.useTriggerDetection)
        {
            EnterClimb(surface);
        }
    }

    private void EnterClimb(ClimbableSurface surface)
    {
        currentSurface = surface;
        isClimbing = true;

        movement?.LockMovement();

        // Align character to entry point if defined, otherwise snap to surface
        if (surface.entryPoint != null)
        {
            controller.enabled = false;
            transform.position = surface.entryPoint.position;
            if (surface.surfaceForward != null)
            {
                transform.rotation = Quaternion.LookRotation(surface.surfaceForward.forward);
            }
            controller.enabled = true;
        }
        else
        {
            climbMovement?.SnapToSurface(surface);
        }

        // Optional: future animation hook
        //animation?.PlayClimbLoop(surface.climbType);
    }

    public void ExitClimb()
    {
        isClimbing = false;
        climbNormal = Vector3.zero;
        currentSurface = null;
        moveInput = Vector2.zero;

        movement?.UnlockMovement();

        // Optional: future animation exit logic
    }

    public void SetInput(Vector2 inputDirection)
    {
        moveInput = inputDirection;
    }

    // ──────────────────────────────────────────────────────────────
    // 🔧 Debug Gizmos
    // ──────────────────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // ──────────────────────────────────────────────────────────────
    // 🔮 Future Extensions (Stubs)
    // ──────────────────────────────────────────────────────────────

    public void TryGrabLedge()
    {
        // Future stub for ledge grab support based on tags or hit position
    }
}
