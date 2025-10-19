using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipInventorySO : ScriptableObject
{
    [SerializeField]
    private AllObjectTypesSO dataBase;
    private readonly Dictionary<ItemNames, int> inventory = new();
    
    [ContextMenu("Debug Inventory")]
    public void DebugMensaje()
    {
        Debug.Log($"[ShipInventorySO]:");
        foreach (var kvp in inventory)
        {
            Debug.Log($"\t{kvp.Key} => {kvp.Value}");
        }
    }
    

    public List<InventoryItemDataObjects> GetAllStacksWithItemName(ItemNames itemName)
    {
        int maxStack = dataBase.GetObjectMaxStackSize(itemName);
        int numOfItems = inventory[itemName];
        List<InventoryItemDataObjects> allStacks = new List<InventoryItemDataObjects>();

        while (numOfItems > 0)
        {
            if (numOfItems >= maxStack)
            {
                InventoryItemDataObjects stack = new InventoryItemDataObjects(itemName, maxStack);
                allStacks.Add(stack);
            
                numOfItems -= maxStack;
            }
            else
            {
                InventoryItemDataObjects stack = new InventoryItemDataObjects(itemName, numOfItems);
                allStacks.Add(stack);
                
                numOfItems = 0;
            }
        }
        
        return allStacks;
    }
    
    public InventoryItemDataObjects ExtractAStackOf(ItemNames itemName, int count)
    {
        if (count > dataBase.GetObjectMaxStackSize(itemName))
            count =  dataBase.GetObjectMaxStackSize(itemName);
        
        if (inventory.ContainsKey(itemName))
        {
            return new InventoryItemDataObjects(itemName, Mathf.Min(count, inventory[itemName]));
        }

        return null;
    }

    public InventoryItemDataObjects ExtractAStackOf(ItemNames itemName)
    {
        return ExtractAStackOf(itemName, dataBase.GetObjectMaxStackSize(itemName));
    }

    public InventoryItemDataObjects ExtractAStackOf(InventoryItemDataObjects item)
    {
        return ExtractAStackOf(item.Name, item.Count);
    }

    public void AddToInventory(ItemNames itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }
    }
    
    public void AddToInventory(InventoryItemDataObjects item) => AddToInventory(item.Name, item.Count);

    public void AddToInventory(List<InventoryItemDataObjects> items)
    {
        foreach (InventoryItemDataObjects item in items)
            AddToInventory(item);
    }

    public void EmptyInventory()
    {
        inventory.Clear();
    }

    public List<InventoryItemDataObjects> GetAllStacks()
    {
        List<InventoryItemDataObjects> retrunValues = new();
        
        foreach (var keyValuePair in inventory)
        {
            List<InventoryItemDataObjects> aux = GetAllStacksWithItemName(keyValuePair.Key);
            foreach (var item in aux)
            {
                retrunValues.Add(item);
            }
        }
        
        return retrunValues;
    }
}
