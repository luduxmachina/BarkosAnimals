using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class IslandPositions : MonoBehaviour //lo pdoria hacer a pelo sin monobehaviour y suscribirse a eventos del gameflow pero entonces habria que crearlo mas tarde y uy que pereza
{
    public static IslandPositions instance;
    Dictionary<ItemNames, List<Transform>> itemsDictionary = new Dictionary<ItemNames, List<Transform>>();

    Transform playerTransform;
    Transform cartTransform;
    Transform boatTransform;
    public UnityEvent<Vector3> DangerEvent = new UnityEvent<Vector3>();
    void Awake()
    {
        instance = this;

    }
    public void FireDangerEvent(Vector3 position)
    {
        DangerEvent?.Invoke(position);
    }
    public void AddPosition(ItemNames animalType,Transform tr)
    {
        if(!itemsDictionary.ContainsKey(animalType))
        {
            itemsDictionary[animalType] = new List<Transform>();
        }
        itemsDictionary[animalType].Add(tr);
    }
    public void RemovePosition(ItemNames animalType, Transform tr)
    {
        if (!itemsDictionary.ContainsKey(animalType))
        {
            return;
        }
        itemsDictionary[animalType].Remove(tr);
    }
    public void SetPlayerPosition(Transform tr)
    {
        playerTransform = tr;
    }
    public Transform GetPlayerPosition()
    {
        return playerTransform;
    }
    public void SetCartPosition(Transform tr)
    {
        cartTransform = tr;
    }
    public Transform GetCartPosition()
    {
        return cartTransform;
    }
    public void SetBoatPosition(Transform tr)
    {
        boatTransform = tr;
    }
    public Transform GetBoatPosition()
    {
        return boatTransform;
    }
    public Transform GetClosest(params ItemNames[] types)
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;
        foreach (var type in types)
        {
            if (!itemsDictionary.ContainsKey(type))
            {
                continue;
            }
            foreach (var tr in itemsDictionary[type])
            {
                float dist = Vector3.Distance(playerTransform.position, tr.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closest = tr;
                }
            }
        }   

        return closest;
    }
    public bool HasType(ItemNames type)
    {
        return itemsDictionary.ContainsKey(type) && itemsDictionary[type].Count > 0;
    }
#if UNITY_EDITOR
    [ContextMenu("Debug Positions")]
    public void DebugMensaje()
    {
        Debug.Log($"[IslandPositions]:");
        foreach (var kvp in itemsDictionary)
        {
            Debug.Log($"\t{kvp.Key} => {kvp.Value.Count} positions");
            foreach (var tr in kvp.Value)
            {
                Debug.Log($"\t\tPosition: {tr.position}");
            }
        }
    }
#endif
}
