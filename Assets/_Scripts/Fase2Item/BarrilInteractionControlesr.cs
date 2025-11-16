using UnityEngine;

public class BarrilInteractionControlesr : MonoBehaviour, IInteractable
{
    [SerializeField]ItemNames tipoContenido;
    [SerializeField]int stacksContenido = 20;

    public bool Interact(ItemNames interactorType, GameObject interactor)
    {
        ItemInScene temp = interactor.GetComponent<ItemInScene>();
        CuencoManager cManager = temp.gameObject.GetComponentInParent<CuencoManager>();
        if (cManager != null)
        {
            if(stacksContenido > 0 && !cManager.HayComida())
            {
                cManager.SetFood(tipoContenido);
                stacksContenido--;
                return true;
            }
            else if(cManager.HayComida() && cManager.ComidaEnCuenco() == tipoContenido)
            {
                stacksContenido++;
                cManager.SetFood(ItemNames.None);
            }
        }
        return false;
    }
}
