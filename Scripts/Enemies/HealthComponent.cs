using UnityEngine;
using Sirenix.OdinInspector;

public class HealthComponent : MonoBehaviour
{
    [Title("Health Settings")]
    [SerializeField, MinValue(1), LabelText("Max Health")]
    private float maxHealth = 100f;

    [SerializeField, ReadOnly, ShowInInspector, ProgressBar(0, "maxHealth")]
    [BoxGroup("Runtime"), LabelText("Current Health")]
    private float currentHealth;

    [BoxGroup("UI"), Required("Assign the Health UI prefab used for this unit.")]
    [SerializeField] private GameObject healthUIPrefab;

    [BoxGroup("UI"), ReadOnly]
    [SerializeField] private WorldspaceHealthUI healthUI; // Assigned at runtime

    [BoxGroup("Runtime"), ReadOnly]
    [ShowInInspector] private EnemyComponentCoordinator coordinator;

    [BoxGroup("Health Settings")]
    [LabelText("Immortal (Can't Die)")]
    [SerializeField]
    private bool isImmortal = false;

    [Button("Toggle Immortal"), BoxGroup("Debug Actions")]
    private void ToggleImmortal()
    {
        isImmortal = !isImmortal;
    }

    [BoxGroup("Debug Actions")]
    [Button(ButtonSizes.Medium)]
    private void SimulateDamage10()
    {
        TakeDamage(10f);
    }

    [Button(ButtonSizes.Medium)]
    private void RestoreToFull()
    {
        ResetHealth();
    }

    public event System.Action<float, float> OnHealthChanged; // current, max

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public void Initialize(EnemyComponentCoordinator parent)
    {
        coordinator = parent;
        currentHealth = maxHealth;
    }
    private void Awake()
    {
        if (coordinator == null)
            coordinator = GetComponent<EnemyComponentCoordinator>();
    }


    private void Start()
    {
        currentHealth = maxHealth;

        if (healthUIPrefab != null)
        {
            GameObject uiObj = Instantiate(healthUIPrefab, UIManager.Instance.worldspaceCanvas.transform);
            healthUI = uiObj.GetComponent<WorldspaceHealthUI>();

            healthUI.targetWorldPosition = this.transform;
            healthUI.health = this;

            healthUI.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        if (coordinator != null && coordinator.IsDead) return;

        currentHealth -= amount;

        if (isImmortal)
        {
            currentHealth = Mathf.Max(currentHealth, 1f); // Never drop below 1
        }
        else
        {
            currentHealth = Mathf.Max(currentHealth, 0f);
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (!isImmortal && currentHealth <= 0f)
        {
            coordinator?.MarkDead();
            coordinator?.deathHandler?.HandleDeath();

            if (healthUI != null)
                Destroy(healthUI.gameObject);
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // Optional helper method if you want runtime config
    public void SetMaxHealth(float value)
    {
        maxHealth = Mathf.Max(1f, value);
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetImmortal(bool value)
    {
        isImmortal = value;
    }

    public void RespawnUI()
    {
        if (healthUIPrefab != null && healthUI == null)
        {
            GameObject uiObj = Instantiate(healthUIPrefab, UIManager.Instance.worldspaceCanvas.transform);
            healthUI = uiObj.GetComponent<WorldspaceHealthUI>();

            healthUI.targetWorldPosition = this.transform;
            healthUI.health = this;
            healthUI.UpdateHealth(currentHealth, maxHealth);
        }
    }

}
