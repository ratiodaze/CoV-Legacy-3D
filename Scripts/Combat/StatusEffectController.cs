using UnityEngine;
using System.Collections.Generic;

public class StatusEffectController : MonoBehaviour
{
    private readonly Dictionary<System.Type, StatusEffect> activeEffects = new();

    public T GetEffect<T>() where T : StatusEffect
    {
        if (activeEffects.TryGetValue(typeof(T), out var effect))
            return effect as T;
        return null;
    }

    public bool HasEffect<T>() where T : StatusEffect
    {
        return activeEffects.ContainsKey(typeof(T));
    }

    public void ApplyEffect(StatusEffect effect)
    {
        var type = effect.GetType();

        if (!activeEffects.ContainsKey(type))
        {
            activeEffects[type] = effect;
            effect.OnApply(this);
        }
        else
        {
            activeEffects[type].OnRefresh();
        }
    }

    public void RemoveEffect<T>() where T : StatusEffect
    {
        if (activeEffects.TryGetValue(typeof(T), out var effect))
        {
            effect.OnExpire();
            activeEffects.Remove(typeof(T));
        }
    }

    public void TickEffects(float deltaTime)
    {
        foreach (var effect in activeEffects.Values)
            effect.Tick(deltaTime);
    }
}
