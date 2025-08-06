using UnityEngine;

public class Teleportable : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget;

    public Transform GetTeleportTarget()
    {
        return teleportTarget != null ? teleportTarget : transform;
    }
}
