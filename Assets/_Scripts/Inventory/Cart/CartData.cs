using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CartData : MonoBehaviour, IInventoryData
{
    [SerializeField]
    private AllObjectTypesSO allItemsDataBase;
    [SerializeField]
    private const int MAX_ITEMS = 3;
    
    public UnityEvent<int, InventoryItemDataObjects> onInventoryAdd = new(); 
    public UnityEvent<int, InventoryItemDataObjects> onInventoryRemove = new();
    
    private List<InventoryItemDataObjects> cartInventory = new();
    
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.K))
        //     TryStackItem(ItemNames.Bread, 10);
        // if (Input.GetKeyDown(KeyCode.L))
        //     TryAddItem(ItemNames.Duck);
        // 
        // if (Input.GetKeyDown(KeyCode.R))
        //     EmptyInventory();
    }
    
   
    public bool InventoryIsFull() => cartInventory.Count >= MAX_ITEMS;
    public bool InventoryIsEmpty() => cartInventory.Count == 0;

    public int TryStackItem(InventoryItemDataObjects item) => TryStackItem(item.Name, item.Count);
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
        onInventoryRemove?.Invoke(id, objectToExtract);
        cartInventory.RemoveAt(id);
        return objectToExtract;
    }

    public void EmptyInventory()
    {
        for (int i = cartInventory.Count - 1; i >= 0; i--)
        {
            onInventoryRemove?.Invoke(i, cartInventory[i]);
        }
        
        Debug.Log("Ship inventory cleared");
        Debug.Log("Cart inventory cleared");
        cartInventory.Clear();
    }



    public List<InventoryItemDataObjects> GetAllInventoryObjects() => cartInventory;

    public List<InventoryItemDataObjects> ExtractAllInventoryObjects()
    {
        List<InventoryItemDataObjects> returnList=new();
        returnList.AddRange(cartInventory);
        EmptyInventory();
        return returnList;
    }

    public List<InventoryItemDataObjects> TryStackAllItems(List<InventoryItemDataObjects> objectsToAdd)
    {
        List<InventoryItemDataObjects> returnList = new();

        foreach(var obj in objectsToAdd)
        {
            int leftover = TryStackItem(obj);

            if (leftover > 0)
                returnList.Add(new InventoryItemDataObjects(obj.Name, leftover));
        }

        return returnList;
    }

    private int AddInExistingSlots(ItemNames itemName, int amount)
    {
        int maxStackSize = allItemsDataBase.GetObjectMaxStackSize(itemName);

        // We check for matching items in the ship inventory
        for (int i = 0; i < cartInventory.Count; i++)
        {
            var item = cartInventory[i];
            
            if (item.Name == itemName && item.Count < maxStackSize)
            {
                // We try to add the full stack
                if (TryAddFullAmountToStack(item, amount))
                {
                    onInventoryAdd?.Invoke(i, item);
                    Debug.Log($"Stack all the remaining items of {itemName} with id {i} into a {item.Count} stack with a maximum of {maxStackSize}");
                    return 0;
                }

                // We try to add as much as we can to the stack
                int amountWeCanStack = maxStackSize - item.Count;
                if (TryAddFullAmountToStack(item, amountWeCanStack))
                {
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
            onInventoryAdd?.Invoke(cartInventory.Count - 1, newObj);
            Debug.Log($"Added a stack of {itemName} with id {cartInventory.Count - 1}, with {amount} items");

            amount = overflow;
        }

        // We return the amount of items we could not add to the cart
        return amount;
    }
}