using UnityEngine;

public class BoatInScene : InventoryInScene
{

    [SerializeField]
    ShipData shipData;
    private void Awake()
    {
       this.inventoryData = shipData;
    }

}
