using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIVisualization : MonoBehaviour
{
    [SerializeField]
    private GameObject ItemSlotPrefab;
    
    private List<GameObject> ItemSlots = new();
    private IInventoryData data;

    private void Start()
    {
        data = GetComponentInParent<IInventoryData>();
    }

    public void AddNewItemSlot(int id, InventoryItemDataObjects item)
    {
        GameObject itemSlot = Instantiate(ItemSlotPrefab, transform);
        itemSlot.GetComponent<InventorySlotManager>()?.SetInventoryData(data);
        
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
