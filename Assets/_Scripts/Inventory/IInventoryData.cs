public interface IInventoryData
{
    /// <summary>
    /// Tries to add X amount of items to the inventory. 
    /// First will try to stack the item with existing slots of items of the same type
    /// If there are still items to add left, will try to add them in empty inventory slots
    /// </summary>
    /// <param name="itemName">The type of item wanted to add to the inventory</param>
    /// <param name="amount">The amount fo items of that type wanted to add to the inventory</param>
    /// <returns>The amount of items we could not fit in the inventory</returns>
    int TryStackItem(ItemNames itemName, int amount);

    /// <summary>
    /// Tries to add 1 of the specified item to the inventory.
    /// First will try to stack the item with existing slots of items of the same type
    /// If there are still items to add left, will try to add them in empty inventory slots
    /// </summary>
    /// <param name="itemName">The type of item wanted to add to the inventory</param>
    /// <returns>True if it could add the item to the inventory and False if not</returns>
    bool TryAddItem(ItemNames itemName);

    /// <summary>
    /// Gets the inventoryDataObjects at the indicated position of the inventory
    /// </summary>
    /// <param name="id">The position of the vector wanted to get the inventoryDataObjects</param>
    /// <returns>
    /// A inventoryDataObjects if there's an object at the selected position of the vector inventory. 
    /// null if no object found at the estimated position
    /// </returns>
    InventoryItemDataObjects GetInventoryObjectByIndex(int id);

    /// <summary>
    /// Gets the inventoryDataObjects at the indicated position of the inventory and then it's eliminated from the inventory's inventory
    /// </summary>
    /// <param name="id">The position of the vector wanted to get the inventoryDataObjects</param>
    /// <returns>
    /// A inventoryDataObjects if there's an object at the selected position of the vector inventory. 
    /// null if no object found at the estimated position
    /// </returns>
    InventoryItemDataObjects ExtractInventoryObjectByIndex(int id);

    /// <summary>
    /// Empties the inventory
    /// </summary>
    void EmptyInventory();
}