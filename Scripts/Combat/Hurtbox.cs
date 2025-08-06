using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private MonoBehaviour damageReceiver;

    private IDamageable damageable;

    private void Awake()
    {
        damageable = damageReceiver as IDamageable;
        if (damageable == null)
        {
            Debug.LogError($"[Hurtbox] Assigned object does not implement IDamageable on {gameObject.name}");
        }
    }

    public void ApplyDamage(DamageData damage)
    {
        damageable?.ApplyDamage(damage);
    }
}
