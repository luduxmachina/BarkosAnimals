using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CartToBoatHandler : MonoBehaviour
{
    [SerializeField]
    private ShipData shipData;
    public UnityEvent onTransferComplete; //ponemos un puf o algo lol
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Item")) { return; }
        CartData cartData = other.GetComponentInChildren<CartData>();
        if(cartData == null)
        { return; }
        if (cartData.InventoryIsEmpty())
        { return; }

        var temp =cartData.ExtractAllInventoryObjects();
        shipData.TryStackAllItems(temp);
        onTransferComplete?.Invoke();


    }


}
