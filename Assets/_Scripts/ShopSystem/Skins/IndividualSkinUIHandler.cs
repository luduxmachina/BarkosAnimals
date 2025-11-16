
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class IndividualSkinUIHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject notEnoughCoins;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField]
    private Image previewImage;
    [SerializeField] private GameObject textoEquipado;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject selectButton;

    private string skinName;
    private int skinPrice;
    private bool isOwned;
    private Sprite skinPreview;
    private SkinSO skin;
    private ShopSkinUIHandler shopSkinUIHandler;

    public void UpdateSkinInfo(SkinSO skin, ShopSkinUIHandler shopSkinUIHandler)
    {
        this.skinName = skin.skinName;
        this.skinPrice = skin.price;
        this.skinPreview = skin.previewImage;
        this.skin = skin;
        this.shopSkinUIHandler = shopSkinUIHandler;
        UpdateUI();

    }
    public void SelectSkin()
    {
        Debug.Log("Selecting skin: " + skinName);
        CurrentSkin.currentSkin = skin;
        UpdateUI();
        shopSkinUIHandler.UpdateUI();
    }
    public void TryBuySkin()
    {
        Debug.Log("Trying to buy skin: " + skinName);
        if (MetaCoinHandler.SpendMetaCoins(skinPrice))
        {
            BoughtSkins.BuySkin(skinName);
            SelectSkin();
            Debug.Log("Skin bought: " + skinName);

        }
        else
        {
            notEnoughCoins.SetActive(true);
            Debug.Log("Not enough MetaCoins to buy skin: " + skinName);

        }




    }

    public void UpdateUI()
    {
        nameText.text = skinName;
        priceText.text = skinPrice.ToString();
        previewImage.sprite = skinPreview;
        isOwned = BoughtSkins.IsSkinBought(skinName);

        if (isOwned)
        {
            buyButton.SetActive(false);
            if(CurrentSkin.currentSkin == skin)
            {
                textoEquipado.SetActive(true);
                selectButton.SetActive(false);
            }
            else
            {
                textoEquipado.SetActive(false);
                selectButton.SetActive(true);

            }
        }
        else
        {
            buyButton.SetActive(true);
            selectButton.SetActive(false);
            textoEquipado.SetActive(false);

        }
    }

}
