using UnityEngine;

/// <summary>
/// Attach to any object to make it climbable by characters with a ClimbHandler.
/// Defines climbable behavior and optional entry/exit points.
/// </summary>
public class ClimbableSurface : MonoBehaviour
{
    public enum ClimbType { Ladder, Wall, Vines, Ledge, Other }

    [Header("Climb Settings")]
    public ClimbType climbType = ClimbType.Ladder;

    [Tooltip("Allows freeform movement (up, down, potentially sideways) during climb.")]
    public bool allowFreeMovement = false;

    [Tooltip("Automatically latch on via trigger volume.")]
    public bool useTriggerDetection = false;

    [Header("Alignment & Snap")]
    [Tooltip("Optional point to snap the character to at the start of the climb.")]
    public Transform entryPoint;

    [Tooltip("Optional point to send the character to on climb exit.")]
    public Transform exitPoint;

    [Tooltip("Optional orientation override for aligning the character.")]
    public Transform surfaceForward;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (entryPoint != null)
        {
            Gizmos.DrawSphere(entryPoint.position, 0.1f);
            Gizmos.DrawLine(transform.position, entryPoint.position);
        }

        if (exitPoint != null)
        {
            Gizmos.DrawSphere(exitPoint.position, 0.1f);
            Gizmos.DrawLine(transform.position, exitPoint.position);
        }

        if (surfaceForward != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + surfaceForward.forward * 1f);
        }
    }
}
