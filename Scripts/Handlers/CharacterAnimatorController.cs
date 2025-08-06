using UnityEngine;

public abstract class CharacterAnimatorController : MonoBehaviour
{
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Combat triggers (used by abilities)

    public virtual void TriggerLightAttack() => _animator.SetTrigger("TriggerLightAttack");
    public virtual void TriggerHeavyAttack() => _animator.SetTrigger("TriggerHeavyAttack");
    public virtual void TriggerSkill1() => _animator.SetTrigger("TriggerSkill1");
    public virtual void TriggerSkill2() => _animator.SetTrigger("TriggerSkill2");
    public virtual void TriggerSkill3() => _animator.SetTrigger("TriggerSkill3");
    public virtual void TriggerUltimate() => _animator.SetTrigger("TriggerUltimate");

    // Movement-related
    public virtual void SetMoveBlend(float x, float y)
    {
        _animator.SetFloat("MoveX", x);
        _animator.SetFloat("MoveY", y);
    }
    public virtual void SetIsMoving(bool value) => _animator.SetBool("IsMoving", value);
    public virtual void SetIsGrounded(bool value) => _animator.SetBool("IsGrounded", value);
    public virtual void SetIsDodging(bool value) => _animator.SetBool("IsDodging", value);

    public virtual void ResetAllCombatTriggers()
    {
        _animator.ResetTrigger("TriggerLightAttack");
        _animator.ResetTrigger("TriggerHeavyAttack");
        _animator.ResetTrigger("TriggerReversalDash");
        _animator.ResetTrigger("TriggerUltimate");
        _animator.ResetTrigger("TriggerSkill2");
        _animator.ResetTrigger("TriggerSkill3");
        _animator.SetBool("IsChargingHeavy", false); // clean reset
    }

}
