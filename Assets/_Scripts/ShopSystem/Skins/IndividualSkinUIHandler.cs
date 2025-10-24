
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class IndividualSkinUIHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image previewImage;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject selectButton;

    private string skinName;
    private int skinPrice;
    private bool isOwned;
    private Sprite skinPreview;

    public void UpdateSkinInfo(string skinName, int price, Sprite skinPreview)
    {
        this.skinName = skinName;
        this.skinPrice = price;
        this.skinPreview = skinPreview;
        UpdateUI();

    }
    private void UpdateUI()
    {
        nameText.text = skinName;
        priceText.text = skinPrice.ToString();
        previewImage.sprite = skinPreview;
        isOwned = BoughtSkins.IsSkinBought(skinName);
        if (isOwned)
        {
            buyButton.SetActive(false);
            selectButton.SetActive(true);
        }
        else
        {
            buyButton.SetActive(true);
            selectButton.SetActive(false);
        }
    }
    public void TryBuySkin()
    {
        Debug.Log("Trying to buy skin: " + skinName);
        if(Random.Range(0,1.0f) > 0.5f) //pongamos que lo compra
        {
            BoughtSkins.BuySkin(skinName);

            Debug.Log("Skin bought: " + skinName);
        }
        else
        {
            Debug.Log("Not enough currency to buy skin: " + skinName);
        }
        UpdateUI();
    }
    
}
