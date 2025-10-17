using System.Collections.Generic;

public interface IInventoryUIVisualization
{
    void AddNewItemSlot(int id, InventoryItemDataObjects item);
    void UpdateItemSlot(int id, InventoryItemDataObjects item);
    void RemoveItemSlot(int id, InventoryItemDataObjects item);
    void SetAllItemSlots(List<InventoryItemDataObjects> shipInventory);
}