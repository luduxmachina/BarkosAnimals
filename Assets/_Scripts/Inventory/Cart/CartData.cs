using System;
using System.Collections.Generic;
using UnityEngine;

public class CartData : MonoBehaviour
{
    [SerializeField]
    private AllObjectTypesSO allItemsDataBase;
    
    private List<CartDataObjects> cartInventory = new();
    public const int MAX_ITEMS = 3;

    /// <summary>
    /// </summary>
    /// <returns>True if the cart inventory has all the slots taken, False in every other case</returns>
    public bool InventoryIsFull()
    {
        return cartInventory.Count >= MAX_ITEMS;
    }

    /// <summary>
    /// Tries to add X amount of items to the cart. 
    /// First will try to stack the item with existing slots of items of the same type
    /// If there are still items to add left, will try to add them in empty cart slots
    /// </summary>
    /// <param name="itemName">The type of item wanted to add to the cart</param>
    /// <param name="amount">The amount fo items of that type wanted to add to the cart</param>
    /// <returns>The amount of items we could not fit in the cart </returns>
    public int TryStackItem(ItemNames itemName, int amount)
    {
        // If there are no items to stack, we dont do shit
        if (amount <= 0)
            return 0;

        // We try to stack in existing slots
        amount = AddInExistingCarSlots(itemName, amount);
        if (amount <= 0)
            return 0;

        // We try to stack the remaining items in empty slots
        return AddInEmptyCartSlots(itemName, amount);
    }

    private int AddInExistingCarSlots(ItemNames itemName, int amount)
    {
        // We check for maching items in the cart inventory
        foreach (var item in cartInventory)
        {
            if (item.Name == itemName)
            {
                // If this stack is full, we dont try to add them to the inventory
                if (item.Count >= item.MaxSize)
                    continue;

                // We try to add the full stack
                if (item.TryAdd(amount))
                {
                    Debug.Log($"Stack of {itemName} is now {item.Count} with a maximum of {item.MaxSize}");
                    return 0;
                }

                // We try to add as much as we can to the stack
                int amountWeCanStack = item.MaxSize - (item.Count + amount);
                if (item.TryAdd(amountWeCanStack))
                    Debug.Log($"Stack of {itemName} is now {item.Count} with a maximum of {item.MaxSize}");

                amount = amount - amountWeCanStack;
            }
        }

        return amount;
    }

    private int AddInEmptyCartSlots(ItemNames itemName, int amount)
    {
        // We check if the amount is bigger than the max stack size
        int maxStack = allItemsDataBase.FindItem(itemName).MaxStackSize;
        int overflow = 0;
        if (amount > maxStack)
        {
            overflow = amount - maxStack;
            amount = maxStack;
        }

        // We try to add the items to the cart if there are empty slots
        while (cartInventory.Count < MAX_ITEMS)
        {
            CartDataObjects newObj = new CartDataObjects(itemName, amount, allItemsDataBase);
            cartInventory.Add(newObj);
            Debug.Log($"Added a stack of {itemName} with {amount} items");

            amount = overflow;
            overflow = 0;
            if (amount > maxStack)
            {
                overflow = amount - maxStack;
                amount = maxStack;
            }
        }

        // We return the amount of items we could not add to the cart
        return amount;
    }    
}