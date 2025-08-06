using UnityEngine;

// Entry point for all character-level logic coordination
[RequireComponent(typeof(CharacterController))] // Most characters will use this for grounded movement
public class CharacterCoordinator : MonoBehaviour
{
    // ──────────────────────────────────────────────────────────────
    // 📦 Core Subsystems
    // ──────────────────────────────────────────────────────────────

    [Header("Core Coordinators")]
    public MovementCoordinator movement;
   // public CombatCoordinator combat;
   // public ResourceCoordinator resources;
   // public GearCoordinator gear;
   // public AnimationCoordinator animation;
   // public StateCoordinator state;
  //  public CharacterMetaCoordinator meta;

    [Header("Optional")]
   // public PresentationCoordinator presentation;
   // public MultiplayerModule multiplayer;
    public InputRouter input;

    // ──────────────────────────────────────────────────────────────
    // 🧠 Unity Lifecycle
    // ──────────────────────────────────────────────────────────────

    private void Awake()
    {
        // Initialize coordinators if they aren't manually assigned
        if (movement == null) movement = GetComponentInChildren<MovementCoordinator>();
      //  if (combat == null) combat = GetComponentInChildren<CombatCoordinator>();
      //  if (resources == null) resources = GetComponentInChildren<ResourceCoordinator>();
      //  if (gear == null) gear = GetComponentInChildren<GearCoordinator>();
      //  if (animation == null) animation = GetComponentInChildren<AnimationCoordinator>();
      //  if (state == null) state = GetComponentInChildren<StateCoordinator>();
      //  if (meta == null) meta = GetComponentInChildren<CharacterMetaCoordinator>();

      //  if (presentation == null) presentation = GetComponentInChildren<PresentationCoordinator>();
      //  if (multiplayer == null) multiplayer = GetComponentInChildren<MultiplayerModule>();
        if (input == null) input = GetComponentInChildren<InputRouter>();
    }

    private void Start()
    {
        // Future: Call Start() methods on all coordinators if needed
    }

    private void Update()
    {
        // Optional tick loop for top-level systems (e.g., multiplayer, input)
    }

    private void OnEnable()
    {
        input?.EnableInput();
    }

    private void OnDisable()
    {
        input?.DisableInput();
    }

    // ──────────────────────────────────────────────────────────────
    // 🔁 Public Accessors / Coordinated Requests
    // ──────────────────────────────────────────────────────────────

   // public bool CanPerformAction()
  //  {
        // Example: Use state locks to determine global action availability
   //     return !state.IsStaggered && !state.IsCasting;
  //  }

   // public void CancelCurrentAction()
   // {
        //combat?.TryCancelCurrentAbility();
   // }
}
