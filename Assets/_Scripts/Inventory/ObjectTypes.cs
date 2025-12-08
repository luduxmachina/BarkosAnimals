using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectTypes
{
    [field: SerializeField] public ItemNames Name { get; private set; }
    
    [field: SerializeField] public ItemType Type { get; private set; }
    
    [field: SerializeField] public GameObject Prefab { get; private set; }
    
    [field: SerializeField] public Sprite ItemImage { get; private set; }
    
    [field: SerializeField, Min(1)] public int MaxStackSize { get; private set; }
     
    [field: SerializeField] public List<Restriction> checkRestrictions { get; private set; }
    
    

    public ObjectTypes()
    {
        Name = ItemNames.None;
        Type = ItemType.None;
        MaxStackSize = 1;
        checkRestrictions = new();
    }
    
    public ObjectTypes(ItemNames name, ItemType type, int maxStackSize)
    {
        Name = name;
        Type = type;
        MaxStackSize = maxStackSize;
        checkRestrictions = new();
    }

    public ObjectTypes(ItemNames name, ItemType type, int maxStackSize, List<Restriction> restrictions)
    {
        Name = name;
        Type = type;
        MaxStackSize = maxStackSize;
        checkRestrictions = new();
        foreach (var r in restrictions)
        {
            checkRestrictions.Add(r);
        }
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
    Meat,
    Duck,
    Snake,
    Pangolin,
    Sheep,
    Player,
    Cart,
    Ship,
    Feeder,
    Coal,
    Carrot
}

public enum ItemType
{
    None,
    Wood,
    Plastic,
    Robotic,
    Plushie,
}

