using System;
using Unity.VisualScripting;
using UnityEngine;

public class InformWhenRemovingFromGrid : MonoBehaviour
{
    [SerializeField] private ItemNames itemName;
    private ShipInventorySO shipInventory;

    public void SetShipInventory (ShipInventorySO shipInventory)
    {
        this.shipInventory = shipInventory;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        
        shipInventory.AddToInventory(itemName, 1);
    }
}
