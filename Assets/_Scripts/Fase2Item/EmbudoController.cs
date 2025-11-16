using UnityEngine;
using UnityEngine.Events;

public class EmbudoController : MonoBehaviour, IInteractable
{
    [SerializeField] RecipientController recipiente;

    public bool Interact(ItemNames interactorType, GameObject interactor)
    {
        if (!recipiente.AddStack(interactorType)) return false;

        ItemInScene temp = interactor.GetComponent<ItemInScene>();
        CuencoManager cManager = temp.gameObject.GetComponentInParent<CuencoManager>();
        if (cManager != null)
        {
            cManager.SetFood(ItemNames.None);
            return true;
        }
        return false;
    }
}
