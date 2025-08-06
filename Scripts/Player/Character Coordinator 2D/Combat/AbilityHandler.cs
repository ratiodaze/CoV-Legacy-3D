using UnityEngine;
using System.Collections.Generic;

// Enum for skill types
public enum SkillType
{
    Skill1,
    Skill2,
    Skill3,
    Ultimate
}

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterKit activeKit;

    private Dictionary<SkillType, SkillCooldownData> skillCooldowns; // Stores cooldown state per skill
    private Queue<SkillType> skillIntentBuffer = new(); // Queues skill use requests

    private void Awake()
    {
        // Define cooldown durations
        skillCooldowns = new()
        {
            { SkillType.Skill1, new SkillCooldownData { cooldownDuration = 4f } },
            { SkillType.Skill2, new SkillCooldownData { cooldownDuration = 6f } },
            { SkillType.Skill3, new SkillCooldownData { cooldownDuration = 8f } },
            { SkillType.Ultimate, new SkillCooldownData { cooldownDuration = 12f } }
        };
    }

    public void QueueSkillIntent(SkillType type)
    {
        if (skillIntentBuffer.Count < 1)
            skillIntentBuffer.Enqueue(type);
    }

    public void Tick(float deltaTime)
    {
        UpdateSkillCooldowns(deltaTime);
        ProcessQueuedSkills();
    }

    private void ProcessQueuedSkills()
    {
        if (skillIntentBuffer.Count == 0)
            return;

        SkillType nextSkill = skillIntentBuffer.Dequeue();
        TriggerSkill(nextSkill);
    }

    private void TriggerSkill(SkillType type)
    {
        var data = skillCooldowns[type];

        if (data.IsOnCooldown)
        {
            Debug.Log($"Skill {type} is on cooldown for {data.GetRemainingTime():F1}s");
            return;
        }

        Debug.Log($"Skill {type} used!");
        data.Trigger();

        // Delegate to current character's skill behavior
        if (activeKit != null)
            activeKit.ExecuteSkill(type);
    }


    private void UpdateSkillCooldowns(float deltaTime)
    {
        foreach (var data in skillCooldowns.Values)
            data.Tick(deltaTime);
    }

}
