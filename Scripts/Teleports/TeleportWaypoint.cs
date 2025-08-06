using UnityEngine;

public class TeleportWaypoint : MonoBehaviour
{
    [Header("Waypoint Info")]
    public string waypointName = "Unnamed Waypoint";
    [TextArea] public string description;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    private void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(spawnPoint.position, 0.3f);
            Gizmos.DrawLine(transform.position, spawnPoint.position);
        }
    }
}
