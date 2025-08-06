using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class EnemyComponentCoordinator : MonoBehaviour, IDamageable, IResettable
{
    [Title("Component References")]

    [Required, LabelText("Health Component")]
    public HealthComponent health;

    [Required, LabelText("Stagger Component")]
    public StaggerComponent stagger;

    [LabelText("Death Handler")]
    public DeathHandler deathHandler;

    [LabelText("Animator (Optional)")]
    public Animator animator;

    [Title("Runtime State")]
    [ShowInInspector, ReadOnly]
    public bool IsDead { get; private set; }

    [ShowInInspector, ReadOnly]
    public bool IsStunned { get; private set; }

    [BoxGroup("Debug Actions")]
    [Button(ButtonSizes.Medium), GUIColor(1f, 0.4f, 0.4f)]
    private void MarkDeadDebug()
    {
        MarkDead();
        deathHandler?.HandleDeath();
    }

    [BoxGroup("Debug Actions")]
    [Button(ButtonSizes.Medium), GUIColor(0.8f, 0.8f, 1f)]
    private void StunDebug()
    {
        MarkStunned(1.5f);
    }

    [BoxGroup("Debug Actions")]
    [Button(ButtonSizes.Medium), GUIColor(0.6f, 1f, 0.6f)]
    private void ResetDummyDebug()
    {
        ResetObject();
    }

    public void MarkDead()
    {
        IsDead = true;
    }

    public void MarkStunned(float duration = 1f)
    {
        if (IsStunned) return;

        IsStunned = true;
        StartCoroutine(ClearStunAfterDelay(duration));
    }

    private IEnumerator ClearStunAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        IsStunned = false;
    }

    public void ApplyDamage(DamageData damage)
    {
        if (IsDead) return;

        health?.TakeDamage(damage.amount);
        stagger?.ApplyStagger(damage.stagger);
    }

    public void ResetObject()
    {
        Debug.Log($"[EnemyCoordinator] Resetting {name}");

        IsDead = false;
        IsStunned = false;

        if (animator != null)
        {
            animator.SetBool("IsDead", false);
            //animator.Play("Idle"); // or your default idle state
        }

        health?.ResetHealth();
        stagger?.ResetStagger();
        health?.RespawnUI();

        gameObject.SetActive(true);
    }
}
