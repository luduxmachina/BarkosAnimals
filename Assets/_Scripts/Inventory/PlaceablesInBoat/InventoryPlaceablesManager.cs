using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlaceablesManager : MonoBehaviour
{
    public ShipInventorySO shipInventory;

    [SerializeField] private ItemNames itemType;
    [SerializeField] private GameObject buttonParent;
    [SerializeField] private GameObject gridImputObject;

    private GameObject buttonObject;
    private IGridInput gridInput;
    private bool placing = false;

    private void Awake()
    {
        if (shipInventory == null)
        {
            Debug.LogError($"[InventoryPlaceablesManager] No ship inventory found at [{gameObject.name}]");
            return;
        }

        foreach (Transform button in buttonParent.transform)
        {
            if (button.GetComponent<ButtonsWithType>().name == itemType)
            {
                buttonObject = button.gameObject;
                buttonObject.GetComponent<Button>().onClick.AddListener(StartPlacing);
            }
            else
            {
                button.gameObject.GetComponent<Button>().onClick.AddListener(StopPlacing);
            }
        }

        if (!shipInventory.ContainsItemsOf(itemType))
        {
            buttonObject.SetActive(false);
        }

        gridInput = gridImputObject.GetComponent<IGridInput>();
        shipInventory.OnInventoryUpdated.AddListener(UpdateWithInventory);
        gridInput.OnClick += () => PlaceItemOnGrid();
        
    }

    public void StartPlacing()=> placing = true;
    public void StopPlacing() => placing = false;
    
    public void PlaceItemOnGrid()
    {
        if(!shipInventory.ContainsItemsOf(itemType))
            return;
        
        if(!placing)
            return;
        
        InventoryItemDataObjects stack = shipInventory.ExtractAStackOf(itemType, 1);
        // if (stack.Count <= 0)
        // {
        //     Debug.LogError($"Stack of size [{stack.Count}], type [{itemType}] detected");
        //     return;
        // }
        // stack.Remove(1);
        // 
        // if (stack.Count > 0)
        // {
        //     shipInventory.AddToInventory(stack);
        // }
        // else
        // {
        //     buttonObject.SetActive(false);
        // }
    }

    public void RemoveItemOnGrid()
    {
        shipInventory.AddToInventory(itemType, 1);
        buttonObject.SetActive(true);
    }

    public void UpdateWithInventory(ItemNames updatedItem)
    {
        if (updatedItem == itemType || updatedItem == ItemNames.None)
        {
            if (shipInventory.ContainsItemsOf(itemType))
            {
                buttonObject.SetActive(true);
            }
            else
            {
                buttonObject.SetActive(false);
                gridInput.StopPlacing();
            }
        }
    }
}
