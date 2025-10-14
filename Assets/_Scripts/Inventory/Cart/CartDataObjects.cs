using UnityEngine;

public class CartDataObjects
{
    public ItemNames Name { get; private set; }
    public int MaxSize { get; private set; }
    public int Count { get; private set; }

    public CartDataObjects(ItemNames name, int count, AllObjectTypesSO allItemsDataBase)
    {
        Name = name;
        MaxSize = allItemsDataBase.FindItem(name).MaxStackSize;
        Count = count;
        if (Count > MaxSize)
            Count = MaxSize;

    }

    public bool TryAdd(int amount)
    {
        if (Count + amount > MaxSize)
            return false;
        
        Count += amount;
        return true;
    }
}
