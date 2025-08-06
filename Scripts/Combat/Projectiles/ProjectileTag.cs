using UnityEngine;

public struct ProjectileTag
{
    public GameObject source;      // Who fired it
    public string skillId;         // Optional: used to identify the ability
    public string elementType;     // Optional: Fire, Light, etc.
    public bool isEmpowered;       // Optional: for Echo Reload, etc.
}
