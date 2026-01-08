using UnityEngine;

public class BoatInScene : InventoryInScene
{

    [SerializeField]
    CartData shipData;
    private void Awake()
    {
       this.inventoryData = shipData;
    }

}
