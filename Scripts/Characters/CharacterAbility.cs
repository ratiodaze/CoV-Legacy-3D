using UnityEngine;
using System;

public abstract class CharacterAbility : MonoBehaviour
{
    /// <summary>
    /// Primary activation method. Use this if the ability doesn't care about combo steps.
    /// </summary>
    public virtual void Activate() => Activate(0, null);

    /// <summary>
    /// Preferred activation entry for all combo-aware abilities. Override this in derived classes.
    /// </summary>
    public virtual void Activate(int comboStep, Action onFinished)
    {
        Debug.LogWarning($"{GetType().Name} did not override Activate(int, Action). Using fallback.");
        onFinished?.Invoke();
    }

    /// <summary>
    /// Whether this ability is currently active (for cancel/interruption).
    /// </summary>
    public virtual bool IsActive => false;

    /// <summary>
    /// Whether this ability can be canceled by dodge or other interruptions.
    /// </summary>
    public virtual bool CanBeCanceledByDodge() => false;

    /// <summary>
    /// Attempts to cancel the ability early (called by dodge or system interrupt).
    /// </summary>
    public virtual void Cancel()
    {
        Debug.Log($"{GetType().Name}: Cancel() called, but no logic implemented.");
    }

    /// <summary>
    /// The base cooldown for this ability, returned to the handler for pacing.
    /// </summary>
    public virtual float CooldownDuration => 0f;
}
