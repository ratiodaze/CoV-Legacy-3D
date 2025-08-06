using UnityEngine;

public class CharacterKitCoordinator : MonoBehaviour
{
    [SerializeField] private CharacterKit activeKit;

    private void Awake()
    {
        if (activeKit == null)
        {
            activeKit = GetComponentInChildren<CharacterKit>();
            if (activeKit == null)
                Debug.LogWarning("No active character kit assigned or found in children.");
        }
    }

    public CharacterKit GetActiveKit()
    {
        return activeKit;
    }

    public void SetActiveKit(CharacterKit newKit)
    {
        activeKit = newKit;
    }

    // NEW METHODS: Skill input routing
    public void OnSkillInputStart(SkillType skill)
    {
        if (activeKit != null)
            activeKit.OnSkillInputStart(skill);
    }

    public void OnSkillInputEnd(SkillType skill)
    {
        if (activeKit != null)
            activeKit.OnSkillInputEnd(skill);
    }
}
