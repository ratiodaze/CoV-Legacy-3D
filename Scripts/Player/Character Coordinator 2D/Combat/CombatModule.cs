using UnityEngine;
using Sirenix.OdinInspector;

public class CombatModule : MonoBehaviour
{
    [ReadOnly, BoxGroup("Debug")] public bool isCombatLocked = false;

    private CharacterCoordinator coordinator;

    public void Initialize(CharacterCoordinator coordinator)
    {
        this.coordinator = coordinator;
    }

    public void LockCombat() => isCombatLocked = true;
    public void UnlockCombat() => isCombatLocked = false;

    public void TryLightAttack() { }
    public void TryHeavyAttack() { }
}
