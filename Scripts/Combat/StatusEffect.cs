using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public float duration = 5f;
    protected float timer;

    public virtual void OnApply(StatusEffectController target)
    {
        timer = duration;
    }

    public virtual void OnRefresh()
    {
        timer = duration;
    }

    public virtual void OnExpire()
    {
        // Cleanup or visual removal
    }

    public virtual void Tick(float deltaTime)
    {
        timer -= deltaTime;
        if (timer <= 0f)
            OnExpire(); // You could trigger automatic removal externally
    }
}
