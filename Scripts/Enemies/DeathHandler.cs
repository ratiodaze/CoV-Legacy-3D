using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class DeathHandler : MonoBehaviour
{
    [Title("Death Behavior Settings")]
    [SerializeField, LabelText("Cull on Death (Destroy)")]
    private bool cullOnDeath = true;

    [SerializeField, Required, LabelText("Target To Destroy")]
    private GameObject targetToDestroy;

    [SerializeField, LabelText("Delay Before Destroy (sec)"), ShowIf("cullOnDeath")]
    private float destroyDelay = 2.5f;

    [Title("Corpse Settings")]
    [SerializeField] private GameObject corpsePrefab;

    [SerializeField, LabelText("Visual Root (Corpse Position)")]
    private Transform visualRoot;

    [SerializeField, LabelText("Corpse Spawn Delay (sec)")]
    private float corpseSpawnDelay = 1.5f;

    [Title("Runtime")]
    [ShowInInspector, ReadOnly] private EnemyComponentCoordinator coordinator;

    [BoxGroup("Debug")]
    [Button(ButtonSizes.Medium)]
    private void SimulateDeath()
    {
        HandleDeath();
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


    public void HandleDeath()
    {
        coordinator?.MarkDead();

        if (corpsePrefab != null)
            Invoke(nameof(SpawnCorpse), corpseSpawnDelay);

        if (cullOnDeath && targetToDestroy != null)
        {
            StartCoroutine(DelayedDestroy()); // ✅ Safely delay destruction
        }
        else
        {
            Debug.Log($"[DeathHandler] {gameObject.name} marked dead, but not culled.");
        }

        // Move this outside of destruction flow
        if (coordinator == null)
        {
            Debug.LogError("[DeathHandler] Coordinator is NULL!");
        }
        else if (coordinator.animator == null)
        {
            Debug.LogError("[DeathHandler] Coordinator's Animator is NULL!");
        }
        else
        {
            Debug.Log("[DeathHandler] Setting Animator 'IsDead' = true");
            coordinator.animator.SetBool("IsDead", true);
        }

    }

    private IEnumerator DelayedDestroy()
    {
        yield return null; // ✅ Wait 1 frame
        Destroy(targetToDestroy, destroyDelay); // Still applies configured delay
    }


    private void SpawnCorpse()
    {
        if (corpsePrefab == null) return;

        Vector3 pos = visualRoot != null ? visualRoot.position : transform.position;
        Quaternion rot = visualRoot != null ? visualRoot.rotation : transform.rotation;

        Instantiate(corpsePrefab, pos, rot);
    }
}
