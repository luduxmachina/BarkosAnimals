using UnityEngine;
[RequireComponent(typeof(SimpleGrabbable))]
public class CartInScene : InventoryInScene
{
    [SerializeField]
    CartData cartData;
    SimpleGrabbable grabbable;
    void Awake()
    {
         grabbable = GetComponent<SimpleGrabbable>();
        if (grabbable == null)
        {
            Debug.LogError("Da fuck?");
        }
        grabbable.OnGrab.AddListener(() => {
            grabbable.currentGrabber.gameObject.GetComponent<PlayerMovement>()?.ApplySlow();
            
            });
        grabbable.OnDrop.AddListener(() => {
            grabbable.currentGrabber.gameObject.GetComponent<PlayerMovement>()?.RemoveSlow();

        });
        this.inventoryData = cartData;
    }


}
