using UnityEngine;
using TMPro;

public class DebugHealthBar : MonoBehaviour
{
    [SerializeField] private HealthComponent health;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Update()
    {
        if (health != null && healthText != null)
        {
            healthText.text = $"HP: {health.CurrentHealth:F0}/{health.MaxHealth:F0}";
        }
    }
}
