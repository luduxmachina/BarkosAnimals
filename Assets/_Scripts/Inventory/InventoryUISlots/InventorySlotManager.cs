using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotManager : MonoBehaviour
{
    [SerializeField]
    private Image imageForDisplay;
    [SerializeField]
    private TextMeshProUGUI textForDisplay;
    [SerializeField]
    private AllObjectTypesSO allObjectTypesDB;
    
    private InventoryItemDataObjects itemForDisplay;
    private IInventoryData inventoryData;
    private int id;
    
    public void SetThisSlotAs(int id, InventoryItemDataObjects item)
    {
        this.id = id;
        UpdateThisSlotTo(item);
    }

    public void UpdateThisSlotTo(InventoryItemDataObjects item)
    {
        itemForDisplay = item;
        
        SetImageToItem(item.Name);
        SetCountTo(item.Count);
    }

    public void ItemSlotPressed()
    {
        inventoryData.ExtractInventoryObjectByIndex(id);
        Destroy(gameObject);
    }
    
    public void SetInventoryData(IInventoryData data) => inventoryData = data;
    
    private void SetCountTo(int itemCount)
    {
        textForDisplay.text = $"x{itemCount}";
    }

    private void SetImageToItem(ItemNames itemName)
    {
        imageForDisplay.sprite = allObjectTypesDB.FindItem(itemName).ItemImage;
    }
}
