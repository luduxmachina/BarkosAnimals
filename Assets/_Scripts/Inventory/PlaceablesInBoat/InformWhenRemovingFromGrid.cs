using System;
using Unity.VisualScripting;
using UnityEngine;

public class InformWhenRemovingFromGrid : MonoBehaviour
{
    public ShipInventorySO shipInventory;
    [SerializeField] private ItemNames itemName;

    public void SetShipInventory (ShipInventorySO inventory)
    {
        shipInventory = inventory;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        
        shipInventory?.AddToInventory(itemName, 1);
    }
}
