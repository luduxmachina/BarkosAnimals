using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [Header("Other player Scripts")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInteraction interaction;


    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction interactAction;




    private void Awake()
    {
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
        dashAction = inputActions.FindActionMap("Player").FindAction("Attack");
        interactAction = inputActions.FindActionMap("Player").FindAction("Interact");
    }
    private void Update()
    {
        movement.moveInput = moveAction.ReadValue<Vector2>();
        if (dashAction.WasPressedThisFrame())
        {
            movement.Dash();
        }
        if (interactAction.WasPressedThisFrame())
        {
            interaction.Interact();
        }
    }
   
 
    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Enable();

    }
}
