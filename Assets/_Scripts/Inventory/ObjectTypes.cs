using System;
using UnityEngine;
using UnityEngine.Internal;

[Serializable]
public class ObjectTypes
{
    public ItemNames Name { get; private set; }
    
    public ItemType Type { get; private set; }
     
    [Min(1)]
    public int MaxStackSize { get; private set; }
     
    public bool IsAnimal { get; private set; }
     
    public bool IsCarnivore { get; private set; }
     
    public bool IsFood { get; private set; }

    public ObjectTypes()
    {
        Name = ItemNames.None;
        Type = ItemType.None;
        MaxStackSize = 1;
        IsAnimal = false;
        IsCarnivore = false;
        IsFood = false;
    }
    
    public ObjectTypes(ItemNames name, ItemType type, int maxStackSize, bool isAnimal, bool isCarnivore, bool isFood)
    {
        Name = name;
        Type = type;
        MaxStackSize = maxStackSize;
        IsAnimal = isAnimal;
        IsCarnivore = isCarnivore;
        IsFood = isFood;
    }
}

public enum ItemNames
{
    None,
    Bread,
    Duck,
}

public enum ItemType
{
    None,
    Wood,
    Plastic,
    Robotic,
    Plushie,
}

