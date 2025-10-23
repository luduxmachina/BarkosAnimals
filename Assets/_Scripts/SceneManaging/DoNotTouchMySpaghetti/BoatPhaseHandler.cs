using UnityEngine;
using UnityEngine.Events;
public class BoatPhaseHandler : MonoBehaviour
{
    public UnityEvent OnStartBoatPhase;
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
        OnStartBoatPhase?.Invoke();
    }
}
