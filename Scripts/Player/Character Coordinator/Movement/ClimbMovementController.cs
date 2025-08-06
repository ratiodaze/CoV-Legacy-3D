using UnityEngine;

/// <summary>
/// Handles player motion and alignment while actively climbing a ClimbableSurface.
/// Requires ClimbHandler to manage climb state and pass input.
/// </summary>
[RequireComponent(typeof(ClimbHandler))]
[RequireComponent(typeof(CharacterController))]
public class ClimbMovementController : MonoBehaviour
{
    [Header("Climb Motion Settings")]
    [SerializeField] private float climbSpeed = 2.5f;
    [SerializeField] private float alignLerpSpeed = 10f;

    private ClimbHandler climbHandler;
    private CharacterController controller;

    private void Awake()
    {
        climbHandler = GetComponent<ClimbHandler>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!climbHandler.IsClimbing || climbHandler.CurrentSurface == null)
            return;

        ClimbableSurface surface = climbHandler.CurrentSurface;

        AlignToSurface(surface);
        HandleClimbMovement(surface, climbHandler.MoveInput);
    }

    private void HandleClimbMovement(ClimbableSurface surface, Vector2 input)
    {
        Vector3 climbDirection = Vector3.zero;

        // Always allow up/down motion
        climbDirection.y = input.y * climbSpeed;

        // Optionally allow free movement
        if (surface.allowFreeMovement)
        {
            Vector3 right = transform.right * input.x * (climbSpeed * 0.75f);
            climbDirection += right;
        }

        controller.Move(climbDirection * Time.deltaTime);
    }

    private void AlignToSurface(ClimbableSurface surface)
    {
        if (surface.surfaceForward != null)
        {
            // Rotate player to face the wall
            Vector3 forward = surface.surfaceForward.forward;
            Quaternion targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * alignLerpSpeed);
        }
    }

    public void SnapToSurface(ClimbableSurface surface)
    {
        if (surface == null || surface.surfaceForward == null)
            return;

        Vector3 directionToWall = -surface.surfaceForward.forward;
        Vector3 targetPosition = surface.transform.position + directionToWall * 0.5f;

        controller.enabled = false;
        transform.position = targetPosition;
        transform.rotation = Quaternion.LookRotation(surface.surfaceForward.forward);
        controller.enabled = true;
    }
}
