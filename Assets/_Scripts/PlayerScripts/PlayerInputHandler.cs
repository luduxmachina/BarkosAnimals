using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [Header("Other player Scripts")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInteraction interaction;



    private InputActionMap playerMap;
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction interactAction;
    private InputAction grabAction;
    private InputAction jumpAction;

    private void Awake()
    {
        playerMap = inputActions.FindActionMap("Player");

        moveAction = playerMap.FindAction("Move");
        dashAction = playerMap.FindAction("Dash");
        grabAction = playerMap.FindAction("Grab");
        interactAction = playerMap.FindAction("Interact");
        jumpAction = playerMap.FindAction("Jump");
    }

    private void Update()
    {
        movement.moveInput = moveAction.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        playerMap.Enable();

        dashAction.performed += OnDash;
        interactAction.performed += OnInteract;
        interactAction.canceled += OnInteractCanceled;
        grabAction.performed += OnGrab;
        jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        dashAction.performed -= OnDash;
        interactAction.performed -= OnInteract;
        interactAction.canceled -= OnInteractCanceled;
        grabAction.performed -= OnGrab;
        jumpAction.performed -= OnJump;

        playerMap.Disable();
    }

    private void OnDash(InputAction.CallbackContext ctx) => movement.Dash();
    private void OnInteract(InputAction.CallbackContext ctx) => interaction.Interact();
    private void OnInteractCanceled(InputAction.CallbackContext ctx) => interaction.StopInteractingWithTarget();
    private void OnGrab(InputAction.CallbackContext ctx) => interaction.Grab();
    private void OnJump(InputAction.CallbackContext ctx) => movement.Jump();
}
