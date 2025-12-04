using UnityEngine;
using UnityEngine.UI;

public class ShowTimerImageFill : MonoBehaviour
{
    [SerializeField]
    PhaseTimer phaseTimer;
    [SerializeField]
    Image timerImageFill;
    
    // Update is called once per frame
    void Update()
    {
        float fillAmount = phaseTimer.timeElapsed / phaseTimer.phaseDuration;
        timerImageFill.fillAmount = fillAmount;
    }
}
