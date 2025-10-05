using UnityEngine;
using UnityEngine.Events;


public class InteractsWithPlayer : IPlayerInteractionReciever
{
    [SerializeField]
    public bool isPlayerInteractable = true;
    [SerializeField, HideIf("isPlayerInteractable", false), Tooltip("Eventos cuando el player interactua con el obj.")]
    private UnityEvent OnPlayerInteract;

    [SerializeField, HideIf("isPlayerInteractable", false)]
    protected bool DropObjOnPlayerInteract = false;

    public bool OnPlayerInteraction(out bool shouldDropObj)
    {
        shouldDropObj = false;
        if (!isPlayerInteractable) return false;

        //se va a interactuar
        shouldDropObj = DropObjOnPlayerInteract;
        OnPlayerInteract?.Invoke();
        return true;
    }
}