using UnityEngine;
using UnityEngine.Events;
public class PhaseTimerHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnTimeUp;
    [SerializeField]
    //Timer timerUI;
    public float phaseTime = 60f;
    [SerializeField]
    private bool StartOnStart = false;
    [SerializeField]
    private bool StartWithPhase = true;
    [SerializeField, HideIf("StartWithPhase", false)]
    private LevelPhases phase;
    public bool timerRunning = false;

    [SerializeField, HideIf("timerRunning", false)]
    private float timeLeft = 10000f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (StartOnStart)
        {
            StartTimer();
        }
        if(StartWithPhase)
        {
            GameFlowManager.instance.nextPhaseHandler.GetPhaseEvent(phase).AddListener(StartTimer);
        }
    }

    public void StartTimer() //se puede iniciar desd donde sea
    {
        timeLeft = phaseTime;
        timerRunning = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(!timerRunning) return;
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            timerRunning = false;
            TimesUp();
        }

        //timerUI.UpdateTimerUI();
    }
    public void TimesUp()
    {
        //Hacer otras cosas, en plan una animacion o lo que sea antes de cambiar de fase
        OnTimeUp.Invoke();
    }
}
