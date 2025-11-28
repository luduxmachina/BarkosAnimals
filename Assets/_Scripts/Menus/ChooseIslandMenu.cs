using UnityEngine;

public class ChooseIslandMenu : MonoBehaviour
{
    public void EscogerIsla (int isla)
    {
        Debug.LogError("Esto ya no hace nada, esta clase deberia estar borrada");
       // GameFlowManager.instance.ChooseIsland(isla);
    }

    public void CambiarEscena()
    {
        GameFlowManager.instance.NextPhase();

    }
}