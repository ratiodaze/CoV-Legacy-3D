using UnityEngine;
using System.Collections.Generic;

public class TeleportManager : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Transform player;

    [Header("Available Waypoints")]
    [SerializeField] private List<TeleportWaypoint> allWaypoints = new();

    public static TeleportManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        // Auto-scan scene for waypoints if not manually assigned
        if (allWaypoints.Count == 0)
        {
            allWaypoints.AddRange(Object.FindObjectsByType<TeleportWaypoint>(FindObjectsSortMode.None));
            Debug.Log($"[TeleportManager] Auto-loaded {allWaypoints.Count} waypoints.");
        }

        if (player == null)
        {
            Debug.LogError("[TeleportManager] Player reference not assigned.");
        }
    }

    /// <summary>
    /// Returns a read-only list of all teleport wayponts.
    /// </summary>
    public IReadOnlyList<TeleportWaypoint> GetWaypoint() => allWaypoints.AsReadOnly();

    /// <summary>
    /// Teleports the player to the given waypoint's spawn point.
    /// </summary>
    public void TeleportToWaypoint(TeleportWaypoint waypoint)
    {
        if (player == null || waypoint == null || waypoint.spawnPoint == null)
        {
            Debug.LogWarning("[TeleportManager] Invalid teleport target.");
            return;
        }

        // Determine what object to move (use Teleportable script or just 'player')
        Transform target = player.GetComponent<Teleportable>()?.GetTeleportTarget() ?? player;

        // Safely handle CharacterController if present
        CharacterController controller = target.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        // Correct for controller height so base of capsule sits flush on floor
        Vector3 correctedPosition = waypoint.spawnPoint.position;
        if (controller != null)
        {
            correctedPosition.y += controller.height / 2f;
        }

        target.position = correctedPosition;
        target.rotation = waypoint.spawnPoint.rotation;

        if (controller != null) controller.enabled = true;

        Debug.Log($"[TeleportManager] Teleported to: {waypoint.waypointName}");
    }

}
