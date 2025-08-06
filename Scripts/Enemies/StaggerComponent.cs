using UnityEngine;
using Sirenix.OdinInspector;

public class StaggerComponent : MonoBehaviour
{
    [Title("Stagger Settings")]
    [SerializeField, MinValue(1), LabelText("Stagger Threshold")]
    private float threshold = 100f;

    [SerializeField, MinValue(0), LabelText("Decay Rate (per second)")]
    private float decayRate = 20f;

    [BoxGroup("Runtime"), ShowInInspector, ReadOnly]
    [ProgressBar(0, "threshold", Height = 18)]
    [LabelText("Current Stagger")]
    private float currentStagger;

    [BoxGroup("Runtime"), ReadOnly, ShowInInspector]
    private EnemyComponentCoordinator coordinator;

    [BoxGroup("Debug Actions")]
    [Button(ButtonSizes.Medium)]
    private void SimulateStagger()
    {
        ApplyStagger(threshold * 0.5f);
    }

    [Button(ButtonSizes.Medium)]
    private void ResetStaggerDebug()
    {
        ResetStagger();
    }

    public void Initialize(EnemyComponentCoordinator parent)
    {
        coordinator = parent;
    }

    private void Awake()
    {
        if (coordinator == null)
            coordinator = GetComponent<EnemyComponentCoordinator>();
    }

    private void Update()
    {
        if (!coordinator.IsStunned && currentStagger > 0)
        {
            currentStagger -= decayRate * Time.deltaTime;
            currentStagger = Mathf.Max(currentStagger, 0f);
        }
    }

    public void ApplyStagger(float amount)
    {
        if (coordinator.IsStunned || coordinator.IsDead) return;

        currentStagger += amount;
        Debug.Log($"[StaggerComponent] {gameObject.name} stagger: {currentStagger}/{threshold}");

        if (currentStagger >= threshold)
        {
            currentStagger = 0f;

            if (coordinator.animator != null)
                coordinator.animator.SetTrigger("LightStun");

            coordinator.MarkStunned(1f);
            Debug.Log($"[StaggerComponent] {gameObject.name} is stunned.");
        }
    }

    public void ResetStagger()
    {
        currentStagger = 0f;
    }

    // ✅ Called from DummyConfig to override default threshold
    public void SetThreshold(float value)
    {
        threshold = Mathf.Max(1f, value);
        Debug.Log($"[StaggerComponent] Threshold set to {threshold} by DummyConfig");
    }
}
