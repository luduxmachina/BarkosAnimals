using UnityEngine;
using UnityEngine.Events;

public class EmbudoController : MonoBehaviour, IInteractable
{
    [SerializeField] RecipientController recipiente;

    public bool Interact(ItemNames interactorType, GameObject interactor)
    {
        return recipiente.AddStack(interactorType);
    }
}
