using UnityEngine;

// Custom player-following camera controller with zoom, tilt, and character swap support
public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pivot; // Rotates horizontally and follows the current target
    [SerializeField] private Transform cameraTransform; // The main camera to move (child of pivot)

    [Header("Zoom Settings")]
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomSmoothTime = 0.15f;

    [Header("Pitch Settings")]
    [SerializeField] private float minPitch = 20f;
    [SerializeField] private float maxPitch = 35f;
    [SerializeField] private float pitchTransitionStart = 8f; // Start adjusting pitch at this zoom distance

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float maxYaw = 90f;
    [SerializeField] private float smoothing = 8f;

    [Header("Recentering Settings")]
    [SerializeField] private float deadZoneAngle = 5f;
    [SerializeField] private float recenterSpeed = 2f;

    [Header("Follow Target")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followLerpSpeed = 12f;

    private float targetYaw = 0f;
    private float currentYaw = 0f;
    private float targetZoomDistance = 10f;
    private float currentZoomDistance = 10f;
    private float zoomVelocity = 0f;

    private bool isDragging = false;
    private Vector3 lastMousePosition;

    public void SetFollowTarget(Transform newTarget)
    {
        followTarget = newTarget;
    }

    private void Start()
    {
        targetZoomDistance = Mathf.Clamp(targetZoomDistance, minZoom, maxZoom);
        currentZoomDistance = targetZoomDistance;
    }

    private void Update()
    {
        if (followTarget != null && pivot != null)
        {
            Vector3 targetPos = followTarget.position;
            pivot.position = Vector3.Lerp(pivot.position, targetPos, Time.deltaTime * followLerpSpeed);
        }

        HandleRotation();
        HandleZoomAndPitch();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            targetYaw += delta.x * rotationSpeed;
            targetYaw = Mathf.Clamp(targetYaw, -maxYaw, maxYaw);
            lastMousePosition = Input.mousePosition;
        }
        else if (Mathf.Abs(targetYaw) < deadZoneAngle)
        {
            targetYaw = Mathf.Lerp(targetYaw, 0f, Time.deltaTime * recenterSpeed);
        }

        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * smoothing);

        if (pivot != null)
        {
            pivot.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        }
    }

    private void HandleZoomAndPitch()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetZoomDistance -= scroll * zoomSpeed;
            targetZoomDistance = Mathf.Clamp(targetZoomDistance, minZoom, maxZoom);
        }

        currentZoomDistance = Mathf.SmoothDamp(
            currentZoomDistance,
            targetZoomDistance,
            ref zoomVelocity,
            zoomSmoothTime
        );

        if (cameraTransform != null)
        {
            float pitchT = Mathf.InverseLerp(pitchTransitionStart, minZoom, currentZoomDistance);
            float currentPitch = Mathf.Lerp(maxPitch, minPitch, pitchT);

            Quaternion lookRotation = Quaternion.Euler(currentPitch, 0f, 0f);
            cameraTransform.localRotation = lookRotation;

            Vector3 zoomDirection = lookRotation * Vector3.back;
            Vector3 desiredOffset = zoomDirection * currentZoomDistance;

            cameraTransform.localPosition = desiredOffset;
        }
    }

    public void ResetCameraRotation()
    {
        targetYaw = 0f;
    }
}
