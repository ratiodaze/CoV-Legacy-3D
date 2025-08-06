using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Routes attack input (light/heavy) to the currently active character kit,
/// manages combo state, and enforces lockout windows between attacks.
/// Abilities define their own cooldowns.
/// </summary>
public class BasicAttackHandler : MonoBehaviour
{
    [SerializeField] private CharacterKit activeKit;

    // ──────────────────────────────────────────────────────────────
    // ⏱ Cooldown / Lockout State
    // ──────────────────────────────────────────────────────────────

    private bool isAttacking = false;
    private float cooldownTimer = 0f;

    // ──────────────────────────────────────────────────────────────
    // 🔁 Combo Step Tracking
    // ──────────────────────────────────────────────────────────────

    private int comboStep = 0;
    private float comboResetTimer = 0f;

    [Header("Combo Settings")]
    [SerializeField] private float comboResetTime = 1.2f;

    // ──────────────────────────────────────────────────────────────
    // 📥 Input Buffering
    // ──────────────────────────────────────────────────────────────

    private Queue<AttackType> inputBuffer = new(); // One-slot buffer

    // ──────────────────────────────────────────────────────────────
    // ⚡ Dual Input (e.g., Light + Heavy = Echo Reload)
    // ──────────────────────────────────────────────────────────────

    [Header("Dual Input Detection")]
    [SerializeField] private float dualInputWindow = 0.2f;
    private float dualInputTimer = 0f;
    private bool lightPressed = false;
    private bool heavyPressed = false;

    // ──────────────────────────────────────────────────────────────
    // 🔁 Per-Frame Update
    // ──────────────────────────────────────────────────────────────

    public void Tick(float deltaTime)
    {
        TickTimers(deltaTime);
        ProcessInputBuffer();
        TickComboResetTimer(deltaTime);
        ProcessDualInput(deltaTime);
    }

    private void TickTimers(float deltaTime)
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= deltaTime;
    }

    private void TickComboResetTimer(float deltaTime)
    {
        if (comboResetTimer > 0f)
        {
            comboResetTimer -= deltaTime;
            if (comboResetTimer <= 0f)
            {
                comboStep = 0;
                Debug.Log("Combo reset due to timeout.");
            }
        }
    }

    // ──────────────────────────────────────────────────────────────
    // 🧠 Attack Processing
    // ──────────────────────────────────────────────────────────────

    public void QueueAttackIntent(AttackType type)
    {
        if (activeKit != null && activeKit.IsInputLocked())
            return;

        if (inputBuffer.Count < 1)
            inputBuffer.Enqueue(type);
    }


    private void ProcessInputBuffer()
    {
        if (isAttacking || cooldownTimer > 0f || inputBuffer.Count == 0)
            return;

        AttackType next = inputBuffer.Dequeue();
        StartAttack(next);
    }

    private void StartAttack(AttackType type)
    {
        isAttacking = true;
        comboResetTimer = comboResetTime;

        // 🔁 Ask kit to activate attack and give us the ability that was used
        CharacterAbility triggeredAbility = activeKit.OnAttackButtonPressed(type, comboStep, EndCurrentAttack);

        if (triggeredAbility != null)
        {
            cooldownTimer = triggeredAbility.CooldownDuration;
        }

        // Debug.Log($"Started {type} attack at combo step {comboStep}");
    }

    private void EndCurrentAttack()
    {
        isAttacking = false;
        comboStep = (comboStep + 1) % 3; // You can make this dynamic later (e.g., kit.MaxComboSteps)
    }

    // ──────────────────────────────────────────────────────────────
    // 🔓 Attack Release Logic (e.g., Heavy Charge Cancel)
    // ──────────────────────────────────────────────────────────────

    public void OnAttackButtonReleased(AttackType type)
    {
        activeKit?.OnAttackButtonReleased(type);
    }

    // ──────────────────────────────────────────────────────────────
    // 🤜 Dual-Input Detection (e.g., Echo Reload)
    // ──────────────────────────────────────────────────────────────

    private void ProcessDualInput(float deltaTime)
    {
        if (dualInputTimer > 0f)
        {
            dualInputTimer -= deltaTime;

            if (lightPressed && heavyPressed)
            {
                activeKit?.OnDualAttackInput();

                lightPressed = false;
                heavyPressed = false;
                dualInputTimer = 0f;
            }
        }
    }

    public void RegisterLightInput()
    {
        lightPressed = true;
        dualInputTimer = dualInputWindow;
    }

    public void RegisterHeavyInput()
    {
        heavyPressed = true;
        dualInputTimer = dualInputWindow;
    }

    // ──────────────────────────────────────────────────────────────
    // 📖 Public API
    // ──────────────────────────────────────────────────────────────

    public int GetCurrentComboStep() => comboStep;
    public bool IsAttacking() => isAttacking;
}
