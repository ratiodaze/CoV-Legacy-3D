using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] private float baseAttackSpeed = 1.0f; // Multiplier (1.0 = normal speed)

    public float AttackSpeedMultiplier => baseAttackSpeed * GetAttackSpeedModifiers();

    private float GetAttackSpeedModifiers()
    {
        // TODO: Add logic for passives, gear, buffs
        return 1f;
    }

    // Optional setter for testing/debug
    public void SetBaseAttackSpeed(float value) => baseAttackSpeed = value;
}
