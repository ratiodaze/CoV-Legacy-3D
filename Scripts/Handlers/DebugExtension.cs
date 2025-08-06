using UnityEngine;

public static class DebugExtension
{
    public static void DebugWireSphere(Vector3 position, Color color, float radius = 1f, float duration = 0f)
    {
        Vector3 up = Vector3.up * radius;
        Vector3 right = Vector3.right * radius;
        Vector3 forward = Vector3.forward * radius;

        // Draw three circular rings to form a wire sphere
        DrawCircle(position, up, right, color, duration);
        DrawCircle(position, up, forward, color, duration);
        DrawCircle(position, forward, right, color, duration);
    }

    private static void DrawCircle(Vector3 center, Vector3 axis1, Vector3 axis2, Color color, float duration = 0f, int segments = 32)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + axis1;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 newPoint = center + Mathf.Cos(angle) * axis1 + Mathf.Sin(angle) * axis2;
            Debug.DrawLine(prevPoint, newPoint, color, duration);
            prevPoint = newPoint;
        }
    }
}
