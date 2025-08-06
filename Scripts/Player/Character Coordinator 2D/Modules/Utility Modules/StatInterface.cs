using UnityEngine;
using Sirenix.OdinInspector;

public class StatInterface : MonoBehaviour
{
    private CharacterCoordinator coordinator;

    public void Initialize(CharacterCoordinator coordinator)
    {
        this.coordinator = coordinator;
    }

    public float GetStat(string statId) => 0f;
}
