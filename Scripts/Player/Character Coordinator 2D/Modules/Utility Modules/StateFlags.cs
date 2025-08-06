using UnityEngine;
using Sirenix.OdinInspector;

public class StateFlags : MonoBehaviour
{
    [BoxGroup("Flags")] public bool isGrounded;
    [BoxGroup("Flags")] public bool isSprinting;
    [BoxGroup("Flags")] public bool isBusy;

    private CharacterCoordinator coordinator;

    public void Initialize(CharacterCoordinator coordinator)
    {
        this.coordinator = coordinator;
    }
}
