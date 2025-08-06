using UnityEngine;

/// <summary>
/// Base class for all character-specific kits (input handling, skill routing, identity).
/// </summary>
public abstract class CharacterKit : MonoBehaviour
{
    [SerializeField] protected CharacterStats stats;

    /// <summary>
    /// Returns the character’s combat style (melee, ranged, spell, etc.).
    /// </summary>
    public abstract CombatStyle GetCombatStyle();

    /// <summary>
    /// Called when an attack input is received.
    /// </summary>
    public abstract CharacterAbility OnAttackButtonPressed(AttackType type, int comboStep, System.Action onFinished);

    /// <summary>
    /// Called when an attack input is released (e.g., charged attacks).
    /// </summary>
    public virtual void OnAttackButtonReleased(AttackType type)
    {
        Debug.LogWarning($"{GetType().Name}: OnAttackButtonReleased called but not implemented.");
    }

    /// <summary>
    /// Called when a skill activation input occurs.
    /// </summary>
    public abstract void ExecuteSkill(SkillType type);

    /// <summary>
    /// Skill press/hold start event (used for aim-preview or charging).
    /// </summary>
    public abstract void OnSkillInputStart(SkillType type);

    /// <summary>
    /// Skill input release event (end of charge or cast).
    /// </summary>
    public abstract void OnSkillInputEnd(SkillType type);

    /// <summary>
    /// Called when both light and heavy inputs are detected in quick succession.
    /// </summary>
    public virtual void OnDualAttackInput() { }

    /// <summary>
    /// Attempts to cancel the current action (e.g., when dodging).
    /// </summary>
    public abstract bool TryCancelCurrentAbility();

    /// <summary>
    /// Returns true if the current action can be interrupted by dodging.
    /// </summary>
    public virtual bool CanBeCanceledByDodge() => false;

    /// <summary>
    /// Optionally expose a specific ability reference for UI or systems.
    /// </summary>
    public virtual CharacterAbility GetAbilityByType(SkillType type) => null;

    // ───────────────────────────────────────────────────────
    // 🔒 Input Lock Management
    // ───────────────────────────────────────────────────────

    protected bool isInputLocked = false;

    /// <summary>
    /// Locks or unlocks combat input (attack and skill queueing).
    /// </summary>
    public void SetInputLock(bool locked)
    {
        isInputLocked = locked;
    }

    /// <summary>
    /// Returns whether combat input is currently locked.
    /// </summary>
    public bool IsInputLocked()
    {
        return isInputLocked;
    }


}
