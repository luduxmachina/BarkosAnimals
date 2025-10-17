using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipInventoryUIVisualization : MonoBehaviour, IInventoryUIVisualization
{
    [SerializeField]
    private ShipData shipData;
    [SerializeField]
    private GameObject ItemSlotPrefab;
    private List<GameObject> ItemSlots = new();

    public void AddNewItemSlot(int id, InventoryItemDataObjects item)
    {
        GameObject itemSlot = Instantiate(ItemSlotPrefab, transform);
        itemSlot.GetComponent<InventorySlotManager>()?.SetInventoryData(shipData);
        
        itemSlot.GetComponent<InventorySlotManager>()?.SetThisSlotAs(id, item);
        ItemSlots[id] = itemSlot;
    }

    public void UpdateItemSlot(int id, InventoryItemDataObjects item)
    {
        if (ItemSlots[id] == null)
        {
            AddNewItemSlot(id, item);
        }
        else
        {
            ItemSlots[id].GetComponent<InventorySlotManager>()?.UpdateThisSlotTo(item);
        }
    }
    
    public void RemoveItemSlot(int id, InventoryItemDataObjects item)
    {
        ItemSlots.RemoveAt(id);
    }

    public void SetAllItemSlots(List<InventoryItemDataObjects> shipInventory)
    {
        for (int i = 0; i < shipInventory.Count; i++)
        {
            AddNewItemSlot(i, shipInventory[i]);
        }
    }
}
