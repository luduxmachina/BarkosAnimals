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
    private InputAction grabAction;
    private InputAction jumpAction;




    private void Awake()
    {
        moveAction = inputActions.FindActionMap("Player").FindAction("Move"); //awsd o left joystick
        dashAction = inputActions.FindActionMap("Player").FindAction("Dash"); //C o east
        grabAction = inputActions.FindActionMap("Player").FindAction("Grab"); //F o west
        interactAction = inputActions.FindActionMap("Player").FindAction("Interact"); //E o north
        jumpAction = inputActions.FindActionMap("Player").FindAction("Jump"); //space o south
    }
    private void Update()
    {
        movement.moveInput = moveAction.ReadValue<Vector2>();
  
    }
   
 
    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
        dashAction.performed += ctx => movement.Dash();
        interactAction.performed += ctx => interaction.Interact();
        interactAction.canceled += ctx => interaction.StopInteractingWithTarget();
        grabAction.performed += ctx => interaction.Grab();
        jumpAction.performed += ctx => movement.Jump();
    }
    private void OnDisable()
    {
        dashAction.performed -= ctx => movement.Dash();
        interactAction.performed -= ctx => interaction.Interact();
        interactAction.canceled -= ctx => interaction.StopInteractingWithTarget();
        grabAction.performed -= ctx => interaction.Grab();
        jumpAction.performed -= ctx => movement.Jump();
        Debug.Log("Player Input Disabled");
        inputActions.FindActionMap("Player").Disable();


    }
}
