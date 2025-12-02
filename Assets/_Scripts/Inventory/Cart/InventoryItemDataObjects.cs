using UnityEngine;

public class InventoryItemDataObjects
{
    public ItemNames Name { get; private set; }
    public int Count { get; private set; }

    public InventoryItemDataObjects(ItemNames name, int count)
    {
        Name = name;
        Count = count;
    }

    public void Add(int amount)
    {
        Count += amount;
    }

    public void Remove(int amount)
    {
        Count -= amount;
        if (Count < 0)
        {
            Count = 0;
        }
    }
}
