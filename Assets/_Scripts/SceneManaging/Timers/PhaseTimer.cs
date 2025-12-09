using UnityEngine;
using UnityEngine.Events;

public class PhaseTimer : MonoBehaviour
{
    public UnityEvent OnForwardTime;
    public float phaseDuration = 60f;
    public float timeRemaining; //pal que lo quiera lol
    public float timeElapsed;

    [Header("-----TimesUp------")]
    public UnityEvent OnTimesUp;
    bool TimeIsUp = false;
  
    public float TimesUpDuration = 5.0f;
    public GameObject TimesUpUI;
    [SerializeField]
    private bool StartOnStart = true;

    public bool timeRunning = false;
    private void Start()
    {
        if (StartOnStart)
        {
            StartPhase();
        }
    }
    public void ForwardTime(float seconds)
    {
        timeElapsed += seconds;
        timeRemaining -= seconds;
        OnForwardTime?.Invoke();
    }
    public void StartPhase()
    {
        timeRemaining = phaseDuration;
        timeElapsed = 0f;
        timeRunning = true;
    }
    public void StopPhase()
    {
        timeRunning = false;
    }
    public void ResetPhase()
    {
        timeRemaining = phaseDuration;
        timeElapsed = 0f;
    }
    private void Update()
    {

        if(TimeIsUp)
        {
            TimesUpDuration -= Time.deltaTime;
            if (TimesUpDuration <= 0.0f)
            {
                if(GameFlowManager.instance !=null)
                {
                    GameFlowManager.instance.NextPhase();
                    return;
                }
                
            }
        }
        if (timeRunning)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                timeElapsed += Time.deltaTime;
            }
            else
            {
                timeRemaining = 0f;
                TimesUp();
                return;
            }
        }
    }
    private void TimesUp()
    {

        timeRunning = false;
        TimeIsUp = true;
        OnTimesUp?.Invoke();
        if (TimesUpUI)
        {
            TimesUpUI.SetActive(true);
            
        }
    }

}
