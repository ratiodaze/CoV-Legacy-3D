using UnityEngine;

public struct DamageData
{
    public float amount;
    public float stagger;
    public GameObject source;

    public DamageData(float amount, float stagger = 0f, GameObject source = null)
    {
        this.amount = amount;
        this.stagger = stagger;
        this.source = source;
    }
}
