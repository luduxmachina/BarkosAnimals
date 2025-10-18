using System.Collections.Generic;
using UnityEngine;

public class CartInventoryUIVisualization : MonoBehaviour, IInventoryUIVisualization
{
    [SerializeField]
    private CartData cartData;
    [SerializeField]
    private GameObject ItemSlotPrefab;
    private List<GameObject> ItemSlots = new();
    
    public void AddNewItemSlot(int id, InventoryItemDataObjects item)
    {
        GameObject itemSlot = Instantiate(ItemSlotPrefab, transform);
        itemSlot.GetComponent<InventorySlotManager>()?.SetInventoryData(cartData);
        
        itemSlot.GetComponent<InventorySlotManager>()?.SetThisSlotAs(id, item);
        ItemSlots.Add(itemSlot);
    }

    public void UpdateItemSlot(int id, InventoryItemDataObjects item)
    {
        if (ItemSlots.Count <= id)
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
        if(ItemSlots.Count > id && ItemSlots[id] != null)
            Destroy(ItemSlots[id]);
        
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
