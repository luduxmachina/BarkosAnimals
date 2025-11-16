using TMPro;
using UnityEngine;

public class Phase2TimeVisual : MonoBehaviour
{
   [SerializeField]
   private PhaseTimer phaseTimer;

    [SerializeField]
    private TextMeshProUGUI timeText;
    private void Update()
    {
        float elapsedTime = phaseTimer.timeElapsed;
        int tempE = Mathf.FloorToInt(elapsedTime);
        float totalTIme= phaseTimer.phaseDuration;
        int tempT = Mathf.FloorToInt(totalTIme);
        timeText.text = tempE.ToString() + " m of " + tempT.ToString() + " m";
    }

}
