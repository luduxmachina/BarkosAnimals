using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndividualIslandSelectionUI : MonoBehaviour
{/*
    int islandIndex;
    IslandSelectionUI parentUI;
    [SerializeField]
    TextMeshProUGUI islandNameText;
    [SerializeField]
    Image islandPreviewImage;
    [SerializeField]
    GameObject itemPreviewPrefab;
    [SerializeField]
    Transform animalPreviewParent;
    [SerializeField]
    GameObject selectButton;
    [SerializeField]
    GameObject selectedText;

    public void InitUI(IslandSO isla, int index, AllObjectTypesSO bd, IslandSelectionUI ui)
    {
        parentUI = ui;
        islandIndex = index;
        islandPreviewImage.sprite = isla.previewInfo.previewImage;
        islandNameText.text = isla.islandName;
        if (GameFlowManager.instance.generatedIsland)
        {
            GenerateIslandInfo(isla);
        }
        ShowAnimalPreview(isla, bd);
    }
    private void ShowAnimalPreview(IslandSO isla, AllObjectTypesSO bd)
    {
        //aqui hay que dividir en animales y no animales
        foreach(ItemNames animal in isla.previewInfo.animalesDeLaIsla)
        {
            GameObject go = Instantiate(itemPreviewPrefab, animalPreviewParent);
            Image img = go.GetComponentInChildren<Image>();
            if (img != null)
            {
                img.sprite = bd.GetSprite(animal);
            }
        }
    }
    private void GenerateIslandInfo(IslandSO isla)
    {
        switch (islandIndex)
        {
            case 0:
                isla.previewInfo = new PreviewInfo
                {
                    previewImage = isla.previewInfo.previewImage,
                    animalesDeLaIsla = new ItemNames[] {
                        ItemNames.Duck
                }
                };
                break;
            case 1:
                isla.previewInfo = new PreviewInfo
                {
                    previewImage = isla.previewInfo.previewImage,
                    animalesDeLaIsla = new ItemNames[] {
                        ItemNames.Duck,
                        ItemNames.Duck
                }
                };

                break;
            case 2:
                isla.previewInfo = new PreviewInfo
                {
                    previewImage = isla.previewInfo.previewImage,
                    animalesDeLaIsla = new ItemNames[] {
                        ItemNames.Pangolin,
                        ItemNames.Duck
                }
                };
                break;
            default:
                isla.previewInfo = new PreviewInfo
                {
                    previewImage = isla.previewInfo.previewImage,
                    animalesDeLaIsla = new ItemNames[] {
                        ItemNames.Snake,
                        ItemNames.Pangolin
                }
                };
                break;
        }

    }
    public void SelectIsland()
    {
      //  GameFlowManager.instance.lastSelectedIsland = islandIndex;
      //  parentUI.UpdateUI();
    }
    public void UpdateUI()
    {
        if (GameFlowManager.instance.lastSelectedIsland == islandIndex)
        {
            selectButton.SetActive(false);
            selectedText.SetActive(true);
        }
        else
        {
            selectButton.SetActive(true);
            selectedText.SetActive(false);
        }
    }*/
}
