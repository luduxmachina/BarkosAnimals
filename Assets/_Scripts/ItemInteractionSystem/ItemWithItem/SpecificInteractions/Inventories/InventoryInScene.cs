using UnityEngine;

public class InventoryInScene : MonoBehaviour, IInteractable, IPlayerInteractionReciever
{

    protected IInventoryData inventoryData;
    [SerializeField]
    AllObjectTypesSO allObjectTypes;
    [SerializeField]
    GameObject inventoryUI;
    GameObject lastInteractor;

    void Start()
    {
        inventoryUI.SetActive(false); //por si acaso
    }
    public bool Interact(ItemNames interactorType, GameObject interactor)
    {

        ItemInScene itemInScene = interactor.GetComponentInChildren<ItemInScene>();
        if (itemInScene == null)
        {
            return false;
        }
        int leftOver = AddItemToCartInventory(itemInScene);



        return leftOver == 0;
    }
    public bool OnPlayerInteraction(GameObject playerReference)
    {
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

        inventoryUI.SetActive(true);

    }
    public int AddItemToCartInventory(ItemInScene interactor)
    {
        int amount = interactor.amountInStack;
        ItemNames name = interactor.itemName;

        int leftover = inventoryData.TryStackItem(name, amount);


        interactor.GetInCart(leftover);





        Debug.Log("Item added to cart: " + interactor.ToString() + " obj: " + interactor.name);

        return leftover;
    }
    public void GiveItemToInteractor(int id, InventoryItemDataObjects itemObject)
    {

        //basicamente spawnear el item en la escena y hacer que lo coja el grabber
        if (lastInteractor == null) { return; }
        int amount = itemObject.Count;
        if (amount <= 0) { return; }
        ItemNames name = itemObject.Name;
        IGrabber grabber = lastInteractor.GetComponent<IGrabber>();
        if (grabber == null) { return; }
        GameObject prefabToSpawn = allObjectTypes.GetObjectPrefab(name);

        GameObject spawnedItem = Instantiate(prefabToSpawn, transform.position + Vector3.up, Quaternion.identity);
        ItemInScene itemInScene = spawnedItem.GetComponentInChildren<ItemInScene>();
        if (itemInScene != null)
        {
            itemInScene.amountInStack = amount;

        }
        grabber.DropObj();
        grabber.GrabObject(spawnedItem.GetComponentInChildren<IGrabbable>());



    }
}
