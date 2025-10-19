using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipData : MonoBehaviour, IInventoryData
{
    [SerializeField]
    private AllObjectTypesSO allItemsDataBase;
    [SerializeField]
    private ShipInventorySO shipInventoryDataBase;
    
    private List<InventoryItemDataObjects> shipInventory = new();
    
    public UnityEvent<int, InventoryItemDataObjects> onInventoryAdd = new(); 
    public UnityEvent<int, InventoryItemDataObjects> onInventoryRemove = new();


    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.O))
        //     TryStackItem(ItemNames.Bread, 10);
        // if (Input.GetKeyDown(KeyCode.P))
        //     TryAddItem(ItemNames.Duck);
        // 
        // if (Input.GetKeyDown(KeyCode.R))
        //     EmptyInventory();
    }

    private void OnEnable()
    {
        GetInventoryFormDataBase();
    }

    public int TryStackItem(InventoryItemDataObjects item)
    {
        return TryStackItem(item.Name, item.Count);
    }

    public int TryStackItem(ItemNames itemName, int amount)
    {
        // If there are no items to stack, we don't do shit
        if (amount <= 0)
            return 0;

        // We try to stack in existing slots
        amount = AddInExistingSlots(itemName, amount);
        if (amount <= 0)
            return 0;

        // We try to stack the remaining items in empty slots
        AddInEmptySlots(itemName, amount);
        
        // There are infinite slots in the ship's inventory
        return 0;
    }

    public bool TryAddItem(ItemNames itemName)
    {
        TryStackItem(itemName, 1);
        
        // There are infinite slots in the ship's inventory
        return true;
    }

    public InventoryItemDataObjects GetInventoryObjectByIndex(int id)
    {
        if (id > shipInventory.Count)
            return null;

        return shipInventory[id];
    }

    public InventoryItemDataObjects ExtractInventoryObjectByIndex(int id)
    {
        if (id > shipInventory.Count)
            return null;

        InventoryItemDataObjects objectToExtract = GetInventoryObjectByIndex(id);
        RemoveItemFromDB(objectToExtract);
        onInventoryRemove.Invoke(id, objectToExtract);
        shipInventory.RemoveAt(id);
        
        return objectToExtract;
    }

    public void EmptyInventory()
    {
        for (int i = shipInventory.Count - 1; i >= 0; i--)
        {
            
            onInventoryRemove?.Invoke(i, shipInventory[i]);
        }

        EmptyDB();
        Debug.Log("Ship inventory cleared");
        shipInventory.Clear();
    }

    public int GetCountOfAllItemsOfType(ItemNames itemName)
    {
        int count = 0;

        foreach (InventoryItemDataObjects item in shipInventory)
        {
            if (item.Name == itemName)
                count += item.Count;
        }
        
        return count;
    }

    private int AddInExistingSlots(ItemNames itemName, int amount)
    {
        int maxStackSize = allItemsDataBase.GetObjectMaxStackSize(itemName);

        // We check for matching items in the ship inventory
        for (int i = 0; i < shipInventory.Count; i++)
        {
            var item = shipInventory[i];
            
            if (item.Name == itemName && item.Count < maxStackSize)
            {
                // We try to add the full stack
                if (TryAddFullAmountToStack(item, amount))
                {
                    AddItemsToDB(itemName, amount);
                    onInventoryAdd?.Invoke(i, item);
                    Debug.Log($"Stack all the remaining items of {itemName} with id {i} into a {item.Count} stack with a maximum of {maxStackSize}");
                    return 0;
                }

                // We try to add as much as we can to the stack
                int amountWeCanStack = maxStackSize - item.Count;
                if (TryAddFullAmountToStack(item, amountWeCanStack))
                {
                    AddItemsToDB(itemName, amountWeCanStack);
                    onInventoryAdd?.Invoke(i, item);
                    Debug.Log($"Stack of {itemName} with id {i} is now {item.Count} with a maximum of {maxStackSize}");
                }

                amount = amount - amountWeCanStack;
            }
        }

        return amount;
    }

    private bool TryAddFullAmountToStack(InventoryItemDataObjects item, int amount)
    {
        int maxStackSize = allItemsDataBase.GetObjectMaxStackSize(item.Name);

        bool canFit = amount + item.Count <= maxStackSize;

        if (canFit)
            item.Add(amount);

        return canFit;
    }

    private void AddInEmptySlots(ItemNames itemName, int amount)
    {
        // We check if the amount is bigger than the max stack size
        int maxStack = allItemsDataBase.FindItem(itemName).MaxStackSize;
        int overflow = 0;
        if (amount > maxStack)
        {
            overflow = amount - maxStack;
            amount = maxStack;
        }

        // We add the items to the ship in empty slots
        while (amount > 0)
        {
            InventoryItemDataObjects newObj = new InventoryItemDataObjects(itemName, amount);
            shipInventory.Add(newObj);
            AddItemsToDB(newObj);
            
            onInventoryAdd?.Invoke(shipInventory.Count - 1, newObj);
            Debug.Log($"Added a stack of {itemName} with id {shipInventory.Count - 1}, with {amount} items");

            amount = overflow;
            overflow = 0;
            if (amount > maxStack)
            {
                overflow = amount - maxStack;
                amount = maxStack;
            }
        }
    }
    
    private void GetInventoryFormDataBase()
    {
        var db = shipInventoryDataBase?.GetAllStacks();
        if(db == null)
            return;

        for (int i = 0; i < db.Count; i++)
        {
            shipInventory.Add(db[i]);
            onInventoryAdd?.Invoke(i, db[i]);
        }
    }

    private void AddItemsToDB(ItemNames itemName, int amount) => shipInventoryDataBase.AddToInventory(itemName, amount);
    private void AddItemsToDB(InventoryItemDataObjects itemToAdd) => shipInventoryDataBase.AddToInventory(itemToAdd);
    private void RemoveItemFromDB(InventoryItemDataObjects objectToExtract) => shipInventoryDataBase.ExtractAStackOf(objectToExtract);
    private void EmptyDB() => shipInventoryDataBase.EmptyInventory();
}
