using UnityEngine;
[RequireComponent(typeof(SimpleGrabbable))]
public class CartInScene : MonoBehaviour, IInteractable, IPlayerInteractionReciever
{
    [SerializeField]
    CartData cartData;
    [SerializeField]
    AllObjectTypesSO allObjectTypes;
    [SerializeField]
    GameObject cartUI;
    SimpleGrabbable grabbable;
    GameObject lastInteractor;
    void Awake()
    {
         grabbable = GetComponent<SimpleGrabbable>();
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
    void Start()
    {
        cartUI.SetActive(false); //por si acaso
    }
    public bool Interact(ItemNames interactorType, GameObject interactor)
    {

        ItemInScene itemInScene = interactor.GetComponent<ItemInScene>();
        if(itemInScene == null)
        {
            return false;
        }
        int leftOver= AddItemToCartInventory(itemInScene);

 

        return leftOver==0;
    }
    public bool OnPlayerInteraction(GameObject playerReference)
    {
        Debug.Log("Se ha interactuado con el carro coño)");
        if (playerReference.CompareTag("Player"))
        {
            Debug.Log("Abriendo UI del carro");
            OpenCartUI();
            
        }
        lastInteractor = playerReference;
        return true;
    }

    private void OpenCartUI()
    {

        cartUI.SetActive(true);

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
    public void GiveItemToInteractor(int id, InventoryItemDataObjects itemObject)
    {

        //basicamente spawnear el item en la escena y hacer que lo coja el grabber
        if(lastInteractor == null) { return; }
        int amount = itemObject.Count;
        if(amount <= 0) { return; }
        ItemNames name= itemObject.Name;
        IGrabber grabber = lastInteractor.GetComponent<IGrabber>();
        if (grabber == null) { return; }
        GameObject prefabToSpawn = allObjectTypes.GetObjectPrefab(name);

        GameObject spawnedItem = Instantiate(prefabToSpawn, transform.position + Vector3.up, Quaternion.identity);
        ItemInScene itemInScene = spawnedItem.GetComponentInChildren<ItemInScene>();
        if (itemInScene != null) { 
            itemInScene.amountInStack = amount;

        }
        grabber.DropObj();
        grabber.GrabObject(spawnedItem.GetComponentInChildren<IGrabbable>());



    }


}
