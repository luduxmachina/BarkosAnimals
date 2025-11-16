using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.InputSystem;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction pausePlayerAction;
    private InputAction exitUIACtion;

    public GameObject PausaMenuUI;
    public GameObject Bestiary;
    public GameObject Options;

    
    private void Awake()
    {
        pausePlayerAction = inputActions.FindActionMap("Player").FindAction("Pause"); //awsd o left joystick
        exitUIACtion = inputActions.FindActionMap("UI").FindAction("Exit"); //awsd o left joystick

    }

    private void OnEnable()
    {
        pausePlayerAction.performed += ctx => PauseGame();
        exitUIACtion.performed += ctx => ResumeGame();
    }
    private void OnDisable()
    {
        pausePlayerAction.performed -= ctx => PauseGame();
        exitUIACtion.performed -= ctx => ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        PausaMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        PausaMenuUI.SetActive(false);
        Bestiary.SetActive(false);
        Options.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        GameFlowManager.instance.GoToMainMenu();
    }
}

