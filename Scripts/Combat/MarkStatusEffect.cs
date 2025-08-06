using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Mark")]
public class MarkStatusEffect : StatusEffect
{
    public override void OnApply(StatusEffectController target)
    {
        base.OnApply(target);
        Debug.Log("Mark applied to enemy.");
        // Visual feedback, etc.
    }

    public override void OnExpire()
    {
        Debug.Log("Mark expired.");
        // Remove visuals
    }
}
