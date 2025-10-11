using UnityEngine;
[RequireComponent(typeof(SimpleGrabbable))]
public class CartInScene : MonoBehaviour, IInteractable, IPlayerInteractionReciever
{
   
    void Awake()
    {
        SimpleGrabbable grabbable = GetComponent<SimpleGrabbable>();
        if (grabbable == null)
        {
            Debug.LogError("Da fuck?");
        }
        grabbable.OnGrab.AddListener(() => {
            grabbable.currentGrabber.gameObject.GetComponent<PlayerMovement>()?.ApplySlow();
            
            });
        grabbable.OnDrop.AddListener(() => {
            grabbable.currentGrabber.gameObject.GetComponent<PlayerMovement>()?.RemoveSlow();

        });
    }
    public bool Interact(ItemInteraction interactorType, MonoBehaviour interactor)
    {

        if (IsFull())
        {
            return false; //no se ha metido nada, esta lleno el carro
        }
        AddItemToCartInventory(interactorType, interactor);

 

        return true;
    }
    public bool OnPlayerInteraction()
    {
        OpenCartUI();
        return true;
    }
    public bool IsFull()
    {
        //preguntar al inventario del carro
        return false;
    }
    public void OpenCartUI()
    {
        //El input debe cambiar y tal, hacerlo en otra clase desd aqui la llamo
    }
    public void AddItemToCartInventory(ItemInteraction interactorType, MonoBehaviour interactor)
    {
        GetInCartHandler getInCartHandler = interactor.GetComponent<GetInCartHandler>();
        if (getInCartHandler == null)
        {
            Debug.LogWarning("Alguien ha intentado meterse en el carro sin el handler para ello:" + interactor.gameObject.name);
            return;
        }
        else
        {
            getInCartHandler.GetInCart();
        }

        //hablar aqui con el inventario del carro

        Debug.Log("Item added to cart: " + interactorType.ToString() + " obj: " + getInCartHandler.name);
    }


}
