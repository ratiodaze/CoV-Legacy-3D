using UnityEngine;
using Sirenix.OdinInspector;

public class TargetingModule : MonoBehaviour
{
    [ReadOnly, BoxGroup("Debug")] public bool isLockingTarget = false;

    private CharacterCoordinator coordinator;

    public void Initialize(CharacterCoordinator coordinator)
    {
        this.coordinator = coordinator;
    }

    public Vector3 GetLookDirection()
    {
        // Placeholder for cursor-facing or lock-on direction
        return Vector3.forward;
    }
}
