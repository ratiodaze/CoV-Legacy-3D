using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 5f;

    private Vector3 direction;
    private float timer = 0f;
    private float damage = 0f;
    private ProjectileTag tag;

    public void Initialize(Vector3 direction, float damage, ProjectileTag tag)
    {
        this.direction = direction.normalized;
        this.damage = damage;
        this.tag = tag;
        timer = 0f;

        // Ignore collisions with owner (if tagged)
        if (tag.source != null)
        {
            Collider ownerCollider = tag.source.GetComponent<Collider>();
            Collider myCollider = GetComponent<Collider>();

            if (ownerCollider != null && myCollider != null)
            {
                Physics.IgnoreCollision(myCollider, ownerCollider, true);
            }
        }
    }


    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for a Hurtbox first
        if (other.TryGetComponent(out Hurtbox hurtbox))
        {
            DamageData hit = new DamageData(damage, 0f, tag.source);
            hurtbox.ApplyDamage(hit);
            Debug.Log($"[Projectile] Hit hurtbox on {other.name}");
        }

        Destroy(gameObject);
    }

}
