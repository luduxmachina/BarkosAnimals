using System.Collections.Generic;
using UnityEngine;

public class ShipData : MonoBehaviour, IInventoryData
{
    [SerializeField]
    private AllObjectTypesSO allItemsDataBase;
    
    private List<InventoryItemDataObjects> shipInventory = new();

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
        shipInventory.RemoveAt(id);
        return objectToExtract;
    }

    public void EmptyInventory()
    {
        Debug.Log("Ship inventory cleared");
        shipInventory.Clear();
    }

    private int AddInExistingSlots(ItemNames itemName, int amount)
    {
        int maxStackSize = allItemsDataBase.GetObjectMaxStackSize(itemName);

        // We check for matching items in the ship inventory
        foreach (var item in shipInventory)
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
                int amountWeCanStack = maxStackSize - (item.Count + amount);
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
            Debug.Log($"Added a stack of {itemName} with {amount} items");

            amount = overflow;
            overflow = 0;
            if (amount > maxStack)
            {
                overflow = amount - maxStack;
                amount = maxStack;
            }
        }
    }
}
