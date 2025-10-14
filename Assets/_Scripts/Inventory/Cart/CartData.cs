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
    /// Tries to add x number of one type of object to the cart
    /// </summary>
    /// <param name="itemName">The name of the object wanted to add</param>
    /// <param name="amount">Number of items wanted to add</param>
    /// <returns>
    /// True if succesfully added the amount specified and False if the amount surpasses the maximum stack size
    /// </returns>
    public bool TryAddItem(ItemNames itemName, int amount)
    {
        // This type of item is already stored in the cart and we can stack this amount in the same inventory slot
        foreach (var item in cartInventory)
        {
            if(item.Name == itemName && item.TryAdd(amount))
                return true;
        }

        // There are empty inventory slots
        if (cartInventory.Count < MAX_ITEMS)
        {
            CartDataObjects newObj = new CartDataObjects(itemName,  amount, allItemsDataBase);
            cartInventory.Add(newObj);
            return true;
        }
        
        // There is no space for this item
        return false;
    }

    public int TryStackItem(ItemNames itemName, int amount)
    {
        foreach (var item in cartInventory)
        {
            if (item.Name == itemName)
            {
                // if this stack is full, we dont try to add them to the inventory
                if (item.Count == item.MaxSize)
                    continue;
                
                // we try to add the full stack
                if (item.TryAdd(amount))
                {
                    Debug.Log($"Stack of {itemName} is now {item.Count} with a maximum of {item.MaxSize}");
                    return 0;
                }
                    

                // we try to add as much as we can to the stack
                int amountWeCanStack = item.MaxSize - (item.Count + amount);
                if (item.TryAdd(amountWeCanStack))
                    Debug.Log($"Stack of {itemName} is now {item.Count} with a maximum of {item.MaxSize}");
                
                return amount - amountWeCanStack;
            }
        }
        
        // There are empty inventory slots
        if (cartInventory.Count < MAX_ITEMS)
        {
            CartDataObjects newObj = new CartDataObjects(itemName,  amount, allItemsDataBase);
            cartInventory.Add(newObj);
            return 0;
        }
        
        return amount;
    }

    public bool IsFull()
    {
        return cartInventory.Count >= MAX_ITEMS;
    }
    
}