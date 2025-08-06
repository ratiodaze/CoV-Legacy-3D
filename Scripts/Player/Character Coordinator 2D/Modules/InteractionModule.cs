using UnityEngine;
using Sirenix.OdinInspector;

public class InteractionModule : MonoBehaviour
{
    [ReadOnly, BoxGroup("Debug")] public bool isInteractionLocked = false;

    private CharacterCoordinator coordinator;

    public void Initialize(CharacterCoordinator coordinator)
    {
        this.coordinator = coordinator;
    }

    public void LockInteraction() => isInteractionLocked = true;
    public void UnlockInteraction() => isInteractionLocked = false;

    public void TryInteract() { }
}
