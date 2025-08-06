using UnityEngine;

public class PlayerLookAtCursor : MonoBehaviour
{
    [SerializeField] private Transform rotateTarget; // Assign to VisualRoot or body pivot
    [SerializeField] private float rotationSpeed = 15f;

    private void Update()
    {
        Vector3? cursorPos = CursorTargetingSystem.Instance.GetCursorGroundPosition();
        if (!cursorPos.HasValue) return;

        Vector3 direction = (cursorPos.Value - rotateTarget.position).normalized;
        direction.y = 0f; // Ignore vertical tilt

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rotateTarget.rotation = Quaternion.Slerp(rotateTarget.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
