using UnityEngine;
using System.Collections.Generic;

public class CursorTargetingSystem : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask targetableLayer;

    [Header("Soft Targeting")]
    [SerializeField] private float softTargetRadius = 2.5f;
    [SerializeField] private float softTargetMaxDistance = 15f;

    private Camera mainCam;
    public static CursorTargetingSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        mainCam = Camera.main;
    }


    /// <summary>
    /// Gets the current ground position the cursor is pointing at.
    /// </summary>
    public Vector3? GetCursorGroundPosition()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            return hit.point;
        }

        return null;
    }

    /// <summary>
    /// Returns a target directly under the cursor, if one exists.
    /// </summary>
    public T GetHardTargetUnderCursor<T>() where T : Component
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetableLayer))
        {
            return hit.collider.GetComponentInParent<T>();
        }

        return null;
    }

    /// <summary>
    /// Attempts to find a valid soft target near the cursorÅfs world point.
    /// </summary>
    public T GetSoftTargetNearCursor<T>() where T : Component
    {
        Vector3? groundPoint = GetCursorGroundPosition();
        if (!groundPoint.HasValue)
            return null;

        Collider[] candidates = Physics.OverlapSphere(groundPoint.Value, softTargetRadius, targetableLayer);
        T bestMatch = null;
        float closestDist = softTargetMaxDistance;

        foreach (var col in candidates)
        {
            T target = col.GetComponentInParent<T>();
            if (target != null)
            {
                float dist = Vector3.Distance(groundPoint.Value, col.transform.position);
                if (dist < closestDist)
                {
                    bestMatch = target;
                    closestDist = dist;
                }
            }
        }

        return bestMatch;
    }

    /// <summary>
    /// Draws debug ray from cursor and soft target radius in scene view.
    /// </summary>
    public void DrawDebugCursorRay()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.cyan);

        Vector3? ground = GetCursorGroundPosition();
        if (ground.HasValue)
        {
            DebugExtension.DebugWireSphere(ground.Value, Color.yellow, softTargetRadius, 1f);
        }
    }

}
