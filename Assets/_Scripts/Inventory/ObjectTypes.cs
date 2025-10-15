using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Internal;

[Serializable]
public class ObjectTypes
{
    [field: SerializeField] public ItemNames Name { get; private set; }
    
    [field: SerializeField] public ItemType Type { get; private set; }
    
    [field: SerializeField] public GameObject Prefab { get; private set; }
    
    [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; }
     
    [field: SerializeField] public bool IsAnimal { get; private set; }
     
    [field: SerializeField] public bool IsCarnivore { get; private set; }
     
    [field: SerializeField] public bool IsFood { get; private set; }
    
    

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

    public override string ToString()
    {
        return Name.ToString();
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

