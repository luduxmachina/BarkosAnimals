using UnityEngine;
using UnityEngine.Events;

public class EmbudoController : MonoBehaviour, IInteractable
{
    [SerializeField] IRecipientControler recipiente;

    private void OnEnable()
    {
        if(recipiente == null)
        {
            recipiente = transform.parent.GetComponentInChildren<IRecipientControler>();
        }
    }

    public bool Interact(ItemNames interactorType, GameObject interactor)
    {

        ItemInScene temp = interactor.GetComponent<ItemInScene>();
        CuencoManager cManager = temp.gameObject.GetComponentInParent<CuencoManager>();

        if (cManager != null ){

            if (!recipiente.AddStack(cManager.TipoComida)) return false;
            cManager.SetFood(ItemNames.None);
            return true;
        }
        return false;
    }
}
