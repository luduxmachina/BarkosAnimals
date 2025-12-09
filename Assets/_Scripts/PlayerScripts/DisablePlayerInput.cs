using UnityEngine;
using UnityEngine.InputSystem;

public class DisablePlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private void Start()
    {
        inputActions.FindActionMap("Player").Disable();
    }
    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Disable();

    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Enable();

    }
}
