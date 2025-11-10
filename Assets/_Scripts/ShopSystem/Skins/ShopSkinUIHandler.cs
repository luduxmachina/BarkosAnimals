using TMPro;
using UnityEngine;

public class ShopSkinUIHandler : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private GameObject individualSkinUIPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private TextMeshProUGUI monedasActuales;
    [SerializeField] private bool useGivenSkingUIs = true;
    [SerializeField, HideIf("useGivenSkingUIs", false)] private IndividualSkinUIHandler[] skinUIs;
    private void Start()
    {
        LoadAndDisplaySkins();
    }
    private void Update()
    {
        monedasActuales.text = MetaCoinHandler.metaCoinCount.ToString();
    }
    private void LoadAndDisplaySkins()
    {
        
        SkinSO[] allSkins = Resources.LoadAll<SkinSO>("Skins");
        if (!useGivenSkingUIs)
        {
            int index = 0;
            foreach (SkinSO skin in allSkins)
            {
                // Instantiate prefab
                GameObject newSkinUI = Instantiate(individualSkinUIPrefab, contentParent);

                skinUIs[index] = newSkinUI.GetComponentInChildren<IndividualSkinUIHandler>();

            }
        }
        for (int i = 0; i < skinUIs.Length && i<allSkins.Length; i++)
        {
            skinUIs[i].UpdateSkinInfo(allSkins[i], this);
        }

    }
    public void UpdateUI()
    {
        foreach (IndividualSkinUIHandler skinUI in skinUIs)
        {
            skinUI.UpdateUI();
        }
    }
}
