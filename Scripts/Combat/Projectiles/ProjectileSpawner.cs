using UnityEngine;

public static class ProjectileSpawner
{
    public static void Spawn(Projectile prefab, Vector3 spawnPos, Vector3 direction, float damage, ProjectileTag tag)
    {
        Projectile proj = GameObject.Instantiate(prefab, spawnPos, Quaternion.LookRotation(direction));
        proj.Initialize(direction, damage, tag);
    }
}
