using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlaceablesManager : MonoBehaviour
{
    public ShipInventorySO shipInventory;

    [SerializeField] private ItemNames itemType;
    [SerializeField] private GameObject buttonParent;
    [SerializeField] private GridPlacementManager gridManager;

    private GameObject buttonObject;
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
                gridManager.OnPlaceStructure.AddListener(PlaceItemOnGrid);
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
        
        shipInventory.OnInventoryUpdated.AddListener(UpdateWithInventory);
    }

    public void StartPlacing()
    {
        placing = true;
    } 
    public void StopPlacing()
    {
        placing = false;
    }
    
    private void PlaceItemOnGrid()
    {
        if(!shipInventory.ContainsItemsOf(itemType))
            return;
        
        if(!placing)
            return;
        
        // We remove the object form the db
        shipInventory.ExtractAStackOf(itemType, 1);
    }

    private void UpdateWithInventory(ItemNames updatedItem)
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
                gridManager.StopPlacement();
            }
        }
    }
}
