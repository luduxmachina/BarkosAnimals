using UnityEngine;
[RequireComponent(typeof(Collider))]
public class CartToBoatHandler : MonoBehaviour
{
    [SerializeField]
    private ShipData shipData;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Item")) { return; }
        CartData cartData = other.GetComponent<CartData>();
        if(cartData == null)
        { return; }
       // if(cartData.IsEmpty())
      //  { return; }

        Debug.Log("inventario del carro al barco");
      //  shipData.TransferInventoryFromCart(cartData);


    }


}
