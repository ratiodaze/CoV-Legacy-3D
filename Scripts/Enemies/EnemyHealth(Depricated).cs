using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;                 // Enemy's total health
    private float currentHealth;

    [SerializeField] private Animator animator;                      // Reference to Animator for triggering animations
    [SerializeField] private GameObject corpsePrefab;                // Manually assigned corpse prefab for this enemy

    private bool isDead = false;                                     // Prevents death from triggering multiple times

    // --- Stagger System ---
    [SerializeField] private float staggerThreshold = 100f;     // How much stagger buildup is needed to trigger a stun
    private float currentStagger = 0f;                          // Current stagger value
    [SerializeField] private float staggerDecayRate = 20f;      // How fast stagger drains over time
    [SerializeField] private float stunDuration = 1f;           // How long the enemy is stunned once threshold is hit
    private bool isStunned = false;                             // Whether enemy is currently stunned
    private float stunTimer = 0f;

    [SerializeField] private Transform visualRoot; // Drag the enemy model or animated rig here


    private void Start()
    {
        currentHealth = maxHealth;

        // If animator is not set, try to auto-assign it from a child object
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        HandleStunTimers();
        DrainStaggerOverTime();
    }

    // Called externally when this enemy is hit by a player attack
    public void TakeDamage(float damage, float staggerAmount = 0f)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage.");

        if (currentHealth <= 0f)
            Die();

        // Only contribute to stagger if not already stunned
        if (!isStunned && staggerAmount > 0f)
        {
            currentStagger += staggerAmount;
            Debug.Log($"{gameObject.name} stagger meter: {currentStagger}/{staggerThreshold}");

            if (currentStagger >= staggerThreshold)
            {
                TriggerStun();
            }
        }
    }

    private void TriggerStun()
    {
        isStunned = true;
        stunTimer = stunDuration;
        currentStagger = 0f; // Reset stagger meter on stun

        if (animator != null)
        {
            animator.SetTrigger("LightStun"); // Could be swapped for "HeavyStun" if needed
        }

        Debug.Log($"{gameObject.name} is stunned for {stunDuration} seconds.");

        // TODO: Add logic to interrupt enemy attack or movement
        // For now, this flag just pauses behavior
    }

    private void HandleStunTimers()
    {
        if (!isStunned) return;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            isStunned = false;
            Debug.Log($"{gameObject.name} recovered from stun.");
        }
    }

    private void DrainStaggerOverTime()
    {
        if (!isStunned && currentStagger > 0f)
        {
            currentStagger -= staggerDecayRate * Time.deltaTime;
            currentStagger = Mathf.Max(currentStagger, 0f);
        }
    }



    // Triggers death animation and starts the transition to the corpse
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} is dying...");

        if (animator != null)
            animator.SetBool("IsDead", true); // Start death animation

        // Wait for the animation to finish before replacing with corpse
        Invoke(nameof(SpawnCorpse), 5f); // Adjust timing based on animation length
    }

    // Spawns the corpse prefab at the same position and rotation, then destroys the enemy
    private void SpawnCorpse()
    {
        if (corpsePrefab != null)
        {
            // Use the visual mesh's rotation and position
            Vector3 spawnPos = visualRoot != null ? visualRoot.position : transform.position;
            Quaternion spawnRot = visualRoot != null ? visualRoot.rotation : transform.rotation;

            Instantiate(corpsePrefab, spawnPos, spawnRot);
            Debug.Log($"{gameObject.name} has been replaced by a corpse.");
        }
        else
        {
            Debug.LogWarning("No corpse prefab assigned to enemy!");
        }

        // Destroy the enemy GameObject after spawning the corpse
        Destroy(gameObject);
    }

}
