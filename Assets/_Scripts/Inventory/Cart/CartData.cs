using System;
using System.Collections.Generic;
using UnityEngine;

public class CartData : MonoBehaviour, IInventoryData
{
    [SerializeField]
    private AllObjectTypesSO allItemsDataBase;
    [SerializeField]
    private const int MAX_ITEMS = 3;
    
    private List<InventoryItemDataObjects> cartInventory = new();
    
    /// <summary>
    /// Calculates if all the inventory slots are taken
    /// </summary>
    /// <returns>True if the inventory has all the slots taken, False in every other case</returns>
    public bool InventoryIsFull()
    {
        return cartInventory.Count >= MAX_ITEMS;
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
        return AddInEmptySlots(itemName, amount);
    }
    
    public bool TryAddItem(ItemNames itemName)
    {
        return TryStackItem(itemName, 1) == 0;
    }
    
    public InventoryItemDataObjects GetInventoryObjectByIndex(int id)
    {
        if (id > cartInventory.Count)
            return null;

        return cartInventory[id];
    }
    
    public InventoryItemDataObjects ExtractInventoryObjectByIndex(int id)
    {
        if (id > cartInventory.Count)
            return null;

        InventoryItemDataObjects objectToExtract = GetInventoryObjectByIndex(id);
        cartInventory.RemoveAt(id);
        return objectToExtract;
    }

    public void EmptyInventory()
    {
        Debug.Log("Cart inventory cleared");
        cartInventory.Clear();
    }

    private int AddInExistingSlots(ItemNames itemName, int amount)
    {
        int maxStackSize = allItemsDataBase.GetObjectMaxStackSize(itemName);

        // We check for matching items in the cart inventory
        foreach (var item in cartInventory)
        {
            if (item.Name == itemName)
            {
                // If this stack is full, we don't try to add them to the inventory
                if (item.Count >= maxStackSize)
                    continue;

                // We try to add the full stack
                if (TryAddFullAmountToStack(item, amount))
                {
                    Debug.Log($"Stack of {itemName} is now {item.Count} with a maximum of {maxStackSize}");
                    return 0;
                }

                // We try to add as much as we can to the stack
                int amountWeCanStack = (item.Count + amount) - maxStackSize;
                if (TryAddFullAmountToStack(item, amountWeCanStack))
                    Debug.Log($"Stack of {itemName} is now {item.Count} with a maximum of {maxStackSize}");

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

    private int AddInEmptySlots(ItemNames itemName, int amount)
    {
        // We check if the amount is bigger than the max stack size
        int maxStack = allItemsDataBase.GetObjectMaxStackSize(itemName);

        // We try to add the items to the cart if there are empty slots
        while (cartInventory.Count < MAX_ITEMS && amount > 0)
        {
            int overflow = 0;
            if (amount > maxStack)
            {
                overflow = amount - maxStack;
                amount = maxStack;
            }

            InventoryItemDataObjects newObj = new InventoryItemDataObjects(itemName, amount);
            cartInventory.Add(newObj);
            Debug.Log($"Added a stack of {itemName} with {amount} items");

            amount = overflow;
        }

        // We return the amount of items we could not add to the cart
        return amount;
    }    
}