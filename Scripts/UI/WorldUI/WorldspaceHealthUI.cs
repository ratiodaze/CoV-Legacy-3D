using UnityEngine;
using TMPro;

public class WorldspaceHealthUI : MonoBehaviour
{
    [SerializeField] public HealthComponent health;

    public Transform targetWorldPosition; // Assigned in code
    public Vector3 offset = new Vector3(0, 2f, 0); // Y offset above enemy
    public TextMeshProUGUI healthText;

    private Camera mainCamera;

    private void Start()
    {
        if (health != null)
        {
            health.OnHealthChanged += UpdateHealth;
            UpdateHealth(health.CurrentHealth, health.MaxHealth); // Set initial display
        }

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (targetWorldPosition == null) return;

        // Convert world position to screen position
        Vector3 worldPos = targetWorldPosition.position + offset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        // Move the UI to this screen position
        transform.position = screenPos;
        //Debug.Log($"Tracking {targetWorldPosition?.name} at screen pos {transform.position}");
    }

    public void UpdateHealth(float current, float max)
    {
        healthText.text = $"HP: {current:F0}/{max:F0}";
    }
    private void OnDestroy()
    {
        if (health != null)
            health.OnHealthChanged -= UpdateHealth;
    }

}
