using UnityEngine;

public class ShopSkinUIHandler : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private GameObject individualSkinUIPrefab;
    [SerializeField] private Transform contentParent;

    private void Start()
    {
        LoadAndDisplaySkins();
    }

    private void LoadAndDisplaySkins()
    {
        // Load all Skin ScriptableObjects from the Resources/Skins folder
        SkinSO[] allSkins = Resources.LoadAll<SkinSO>("Skins");

        foreach (SkinSO skin in allSkins)
        {
            // Instantiate prefab
            GameObject newSkinUI = Instantiate(individualSkinUIPrefab, contentParent);

            // Get the UI handler
            IndividualSkinUIHandler skinUIHandler = newSkinUI.GetComponentInChildren<IndividualSkinUIHandler>();

            if (skinUIHandler != null)
            {

                skinUIHandler.UpdateSkinInfo(skin.skinName, skin.price);
            }
            else
            {
                Destroy(newSkinUI);
            }
        }
    }
}
