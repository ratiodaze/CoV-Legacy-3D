using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

// Routes player input into coordinator systems
public class InputRouter : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementCoordinator movement;
    [SerializeField] private ClimbHandler climbHandler;

    [Header("Input Actions")]
    [SerializeField] private PlayerControls controls;

    [ShowInInspector, ReadOnly] private Vector2 moveInput;

    // ──────────────────────────────────────────────────────────────
    // 🧠 Unity Lifecycle
    // ──────────────────────────────────────────────────────────────

    private void Awake()
    {
        if (controls == null)
            controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.Dash.performed += ctx => movement?.TriggerDash();
        controls.Gameplay.Jump.performed += ctx => movement?.TriggerJump();

        controls.Gameplay.Interact.performed += ctx => climbHandler?.TryBeginClimbFromInteract();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (climbHandler != null && climbHandler.IsClimbing)
        {
            climbHandler.SetInput(moveInput);
        }
        else
        {
            movement?.SetInputDirection(moveInput);
        }
    }

    // Optional access for debug tools or external overrides
    public Vector2 GetMoveInput() => moveInput;

    public void EnableInput() => controls?.Enable();
    public void DisableInput() => controls?.Disable();
}
