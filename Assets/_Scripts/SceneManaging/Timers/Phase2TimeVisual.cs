using TMPro;
using UnityEngine;

public class Phase2TimeVisual : MonoBehaviour
{
    [SerializeField]
    GameObject extraMetrosTexto;
   [SerializeField]
   private PhaseTimer phaseTimer;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private float ExtraMetrosFadeTime= 3.0f;
    [SerializeField,ReadOnly]
    private float extraMetrosTimer = 0.0f;
    private void Update()
    {
        float elapsedTime = phaseTimer.timeElapsed;
        int tempE = Mathf.FloorToInt(elapsedTime);
        float totalTIme= phaseTimer.phaseDuration;
        int tempT = Mathf.FloorToInt(totalTIme);
        timeText.text = tempE.ToString() + " m of " + tempT.ToString() + " m";

        if (extraMetrosTexto.activeSelf)
        {
            extraMetrosTimer -= Time.deltaTime;
            if (extraMetrosTimer <= 0.0f)
            {
                extraMetrosTexto.SetActive(false);
            }
        }
    }
    public void ShowExtraMetrosText()
    {
        extraMetrosTexto.SetActive(true);
        extraMetrosTimer = ExtraMetrosFadeTime;
    }

}
