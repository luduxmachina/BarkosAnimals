using UnityEngine;
[RequireComponent(typeof(SimpleGrabbable))]
public class CartInScene : MonoBehaviour, IInteractable, IPlayerInteractionReciever
{
    [SerializeField]
    CartData cartData;
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
    public bool Interact(ItemInteraction interactorType, GameObject interactor)
    {

        ItemInScene itemInScene = interactor.GetComponent<ItemInScene>();
        if(itemInScene == null)
        {
            return false;
        }
        int leftOver= AddItemToCartInventory(itemInScene);

 

        return leftOver==0;
    }
    public bool OnPlayerInteraction()
    {
        OpenCartUI();
        return true;
    }

    public void OpenCartUI()
    {
        //El input debe cambiar y tal, hacerlo en otra clase desd aqui la llamo
    }
    public int AddItemToCartInventory(ItemInScene interactor)
    {
        int amount = interactor.amountInStack;
        ItemNames name= interactor.itemName;

        int leftover = cartData.TryStackItem(name, amount);
        

        interactor.GetInCart(leftover); 





        Debug.Log("Item added to cart: " + interactor.ToString() + " obj: " + interactor.name);

        return leftover;
    }


}
