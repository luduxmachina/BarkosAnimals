using UnityEngine;

public class ChooseIslandMenu : MonoBehaviour
{
    public void EscogerIsla (int isla)
    {
        GameFlowManager.instance.ChooseIsland(isla);
    }

    public void CambiarEscena()
    {
        GameFlowManager.instance.NextPhase();

    }
}