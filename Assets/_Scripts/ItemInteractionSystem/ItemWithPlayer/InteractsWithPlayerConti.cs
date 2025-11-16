using UnityEngine;
using UnityEngine.Events;

public class InteractsWithPlayerConti : MonoBehaviour, IContinuousPlayerInteractionReciever
{
    [SerializeField]
    public bool isPlayerInteractable = true;
    [SerializeField, HideIf("isPlayerInteractable", false), Tooltip("Eventos cuando el player interactua con el obj.")]
    public UnityEvent OnPlayerInteractStart;
    public UnityEvent OnPlayerInteractStop;

    public bool interactionSuccessful = true;

    public bool OnPlayerStartInteraction(GameObject playerReference)
    {

        if (!isPlayerInteractable) return false;


        //se va a interactuar      
        OnPlayerInteractStart?.Invoke();


        return interactionSuccessful;
    }

    public void OnPlayerStopInteraction(GameObject playerReference)
    {
        OnPlayerInteractStop?.Invoke();
    }
}
