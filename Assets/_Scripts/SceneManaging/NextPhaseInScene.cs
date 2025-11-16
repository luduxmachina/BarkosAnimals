using UnityEngine;

public class NextPhaseInScene : MonoBehaviour
{
   
    public void NextPhaseHandler()
    {
        GameFlowManager.instance.NextPhase();
    }
}
