using UnityEngine;
using System;

public class EchoChargeSystem : MonoBehaviour
{
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private float chargeDuration = 15f; // Default duration of each charge

    private float[] chargeTimers;

    public int CurrentCharges { get; private set; } = 0;

    // Events (optional): notify other systems
    public event Action<int> OnChargeGained;
    public event Action<int> OnChargeLost;

    private void Awake()
    {
        chargeTimers = new float[maxCharges];
    }

    private void Update()
    {
        for (int i = 0; i < CurrentCharges; i++)
        {
            chargeTimers[i] -= Time.deltaTime;

            if (chargeTimers[i] <= 0f)
            {
                RemoveCharge(i);
                i--; // Re-check this index after shifting
            }
        }
    }

    public void GainCharge()
    {
        if (CurrentCharges >= maxCharges)
            return;

        chargeTimers[CurrentCharges] = chargeDuration;
        CurrentCharges++;
        OnChargeGained?.Invoke(CurrentCharges);
    }

    public bool ConsumeCharges(int amount)
    {
        if (CurrentCharges < amount)
            return false;

        for (int i = 0; i < amount; i++)
        {
            RemoveCharge(CurrentCharges - 1);
        }

        return true;
    }

    private void RemoveCharge(int index)
    {
        for (int i = index; i < CurrentCharges - 1; i++)
        {
            chargeTimers[i] = chargeTimers[i + 1];
        }

        chargeTimers[CurrentCharges - 1] = 0f;
        CurrentCharges--;
        OnChargeLost?.Invoke(CurrentCharges);
    }
}
