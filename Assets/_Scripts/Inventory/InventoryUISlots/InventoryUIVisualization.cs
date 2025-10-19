using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIVisualization : MonoBehaviour
{
    [SerializeField]
    private GameObject itemSlotPrefab;
    
    [SerializeField]
    private Transform itemSlotParent;
    
    private List<GameObject> itemSlots = new();
    private IInventoryData data;

    private void Start()
    {
        data = GetComponentInParent<IInventoryData>();
    }

    public void AddNewItemSlot(int id, InventoryItemDataObjects item)
    {
        GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotParent);
        itemSlot.GetComponent<InventorySlotManager>()?.SetInventoryData(data);
        
        itemSlot.GetComponent<InventorySlotManager>()?.SetThisSlotAs(id, item);
        itemSlots.Add(itemSlot);
    }

    public void UpdateItemSlot(int id, InventoryItemDataObjects item)
    {
        if (itemSlots.Count <= id)
        {
            AddNewItemSlot(id, item);
        }
        else
        {
            itemSlots[id].GetComponent<InventorySlotManager>()?.UpdateThisSlotTo(item);
        }
    }
    
    public void RemoveItemSlot(int id, InventoryItemDataObjects item)
    {
        if(itemSlots.Count > id && itemSlots[id] != null)
            Destroy(itemSlots[id]);
        
        itemSlots.RemoveAt(id);
    }

    public void SetAllItemSlots(List<InventoryItemDataObjects> shipInventory)
    {
        for (int i = 0; i < shipInventory.Count; i++)
        {
            AddNewItemSlot(i, shipInventory[i]);
        }
    }
}
