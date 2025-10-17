using UnityEngine;
using UnityEngine.Events;


public class InteractsWithPlayer : MonoBehaviour, IPlayerInteractionReciever
{
    [SerializeField]
    public bool isPlayerInteractable = true;
    [SerializeField, HideIf("isPlayerInteractable", false), Tooltip("Eventos cuando el player interactua con el obj.")]
    public UnityEvent OnPlayerInteract;
    public bool interactionSuccessful = true;

    public bool OnPlayerInteraction()
    {
      
        if (!isPlayerInteractable) return false;


        //se va a interactuar      
        OnPlayerInteract?.Invoke();


        return interactionSuccessful;
    }
}