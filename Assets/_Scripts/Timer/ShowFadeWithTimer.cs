using UnityEngine;

public class ShowFadeWithTimer : MonoBehaviour
{

    [SerializeField]
    private Fade fade;
    [SerializeField]
    private PhaseTimer phaseTimer;
    [SerializeField, ReadOnly]
    private float timeUntilFade;
    [SerializeField, ReadOnly]
    private bool timeUp= false;
    private void OnEnable()
    {
        phaseTimer.OnTimesUp.AddListener(TimeOut);

    }
    private void OnDisable()
    {
        phaseTimer.OnTimesUp.RemoveListener(TimeOut);
    }
    private void TimeOut()
    {
        timeUntilFade = phaseTimer.TimesUpDuration - fade.timeToFade - 0.1f;
        timeUp = true;
    }
    private void Update()
    {
        if (timeUp)
        {
            timeUntilFade -= Time.deltaTime;
            if (timeUntilFade <= 0.0f)
            {
                fade.gameObject.SetActive(true);
                timeUp = false;
            }
        }
    }
    private void Start()
    {
        if(fade == null || phaseTimer ==null)
        {
           Destroy(this);
        }
    }
}
