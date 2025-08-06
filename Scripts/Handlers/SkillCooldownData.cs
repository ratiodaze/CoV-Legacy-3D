using UnityEngine;

[System.Serializable]
public class SkillCooldownData
{
    public float cooldownDuration;
    private float cooldownRemaining;

    public bool IsOnCooldown => cooldownRemaining > 0f;

    public void Tick(float deltaTime)
    {
        if (cooldownRemaining > 0f)
        {
            cooldownRemaining -= deltaTime;
            if (cooldownRemaining < 0f)
                cooldownRemaining = 0f;
        }
    }

    public void Trigger()
    {
        cooldownRemaining = cooldownDuration;
    }

    public float GetRemainingTime()
    {
        return cooldownRemaining;
    }
}
