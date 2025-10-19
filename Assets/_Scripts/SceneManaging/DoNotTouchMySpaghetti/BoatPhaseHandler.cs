using UnityEngine;

public class BoatPhaseHandler : MonoBehaviour
{
    
    private void OnEnable()
    {
        GameFlowManager.instance.onStartBoatPhase.AddListener(HandleStartBoatPhase);
    }
    private void OnDisable()
    {
        GameFlowManager.instance.onStartBoatPhase.AddListener(HandleStartBoatPhase);
    }
    private void HandleStartBoatPhase()
    {
        Debug.Log("Boat Phase Started");
    }
}
