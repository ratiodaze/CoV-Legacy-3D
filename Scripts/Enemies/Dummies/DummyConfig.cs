using UnityEngine;
using Sirenix.OdinInspector;

public class DummyConfig : MonoBehaviour
{
    [Title("Core Dummy Settings")]
    [EnumToggleButtons]
    public DummyType dummyType = DummyType.Basic;

    [ShowIf("dummyType", DummyType.Basic)]
    [BoxGroup("Basic Dummy")]
    public float startingHealth = 100f;

    [ShowIf("dummyType", DummyType.Stagger)]
    [BoxGroup("Stagger Dummy")]
    public float staggerThreshold = 50f;

    [ShowIf("dummyType", DummyType.Immortal)]
    [BoxGroup("Immortal Dummy")]
    [InfoBox("This dummy cannot die. It will stop at 1 HP.")]
    public bool showDamageNumber = true;

    [Title("Debug Buttons")]
    [Button(ButtonSizes.Large)]
    public void TakeDamageTest()
    {
        GetComponent<HealthComponent>()?.TakeDamage(25f);
    }

    [Button(ButtonSizes.Medium)]
    public void ResetDummy()
    {
        var resettable = GetComponent<IResettable>();
        resettable?.ResetObject();
    }
    private void Awake()
    {
        var health = GetComponent<HealthComponent>();
        var stagger = GetComponent<StaggerComponent>();

        switch (dummyType)
        {
            case DummyType.Basic:
                if (health != null)
                    health.SetMaxHealth(startingHealth);
                break;

            case DummyType.Immortal:
                if (health != null)
                {
                    health.SetMaxHealth(startingHealth);
                    health.SetImmortal(true); // You can implement this method
                }
                break;

            case DummyType.Stagger:
                if (health != null)
                    health.SetMaxHealth(startingHealth);

                if (stagger != null)
                    stagger.SetThreshold(staggerThreshold);
                break;
        }
    }

}

public enum DummyType
{
    Basic,
    Immortal,
    Stagger
}
