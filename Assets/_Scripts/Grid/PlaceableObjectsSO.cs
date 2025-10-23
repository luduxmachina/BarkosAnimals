using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlaceableObjectsSO : ScriptableObject
{
    public List<ObjectData> objectData;
    
#if UNITY_EDITOR
    // private void OnValidate()
    // {
    //     if (objectData[objectData.Count - 1].ID <= 0)
    //     {
    //         objectData[objectData.Count - 1].ID = objectData.Count - 1;
    //     }
    // }
#endif
}

[Serializable]
public class ObjectData
{
    [field: SerializeField] public string Name{ get; private set; }
    [field: SerializeField] public int ID{ get; set; }
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField] public GameObject Prefab{ get; private set; }
    
    [field: SerializeField] public Sprite ImageUI{ get; private set; }
}