using System;
using UnityEngine;
[RequireComponent(typeof(IGrabbable))]
public class CartInScene : InventoryInScene
{
    [SerializeField]
    CartSlowEffect slowEffect = new();
    [SerializeField]
    CartData cartData;
    IGrabbable grabbable;

    void Awake()
    {
         grabbable = GetComponent<IGrabbable>();
        if (grabbable == null)
        {
            Debug.LogError("Da fuck?");
        }
        grabbable.OnGrab.AddListener(() => {
            grabbable.currentGrabber.gameObject.GetComponentInChildren<PlayerInSceneEffects>()?.AddEffect(slowEffect);
            
            });
        grabbable.OnDrop.AddListener(() => {
            grabbable.currentGrabber.gameObject.GetComponentInChildren<PlayerInSceneEffects>()?.RemoveEffect(slowEffect);

        });
        this.inventoryData = cartData;
    }


}
[Serializable]
public class CartSlowEffect : PlayerEffect
{
    [SerializeField, Range(0,1)]
    float percentageOfSpeed=1f;

    public override void ApplyEffect(PlayerCurrentStats playerStats)
    {
        playerStats.currentStats.moveSpeed *= percentageOfSpeed;
        playerStats.currentStats.rotationSpeed *= percentageOfSpeed;

    }
}
