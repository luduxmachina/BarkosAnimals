using UnityEngine;
using UnityEngine.Events;
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

    public static UnityEvent OnPause = new UnityEvent();
    public static UnityEvent OnResume = new UnityEvent();
    static bool inittialized = false;

    private void Awake()
    {
        if (inittialized)
        {
            return;
        }
        inittialized = true;
        pausePlayerAction = inputActions.FindActionMap("Player").FindAction("Pause"); //awsd o left joystick
        exitUIACtion = inputActions.FindActionMap("UI").FindAction("Exit"); //awsd o left joystick

        pausePlayerAction.performed += ctx => OnPause?.Invoke();
        exitUIACtion.performed += ctx => OnResume?.Invoke();

    }
    public static void ResumeStatic()
    {
        OnResume?.Invoke();
    }

    private void OnEnable()
    {
       OnPause.AddListener(PauseGame);
        OnResume.AddListener(ResumeGame);
    }
    private void OnDisable()
    {
        OnPause.RemoveListener(PauseGame);
        OnResume.RemoveListener(ResumeGame);
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0f;
        PausaMenuUI.SetActive(true);
       // OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        PausaMenuUI.SetActive(false);
        Bestiary.SetActive(false);
        Options.SetActive(false);
      //  OnResume?.Invoke();
    }

    public void ExitToMainMenu()
    {
        GameFlowManager.instance.GoToMainMenu();
    }
}

