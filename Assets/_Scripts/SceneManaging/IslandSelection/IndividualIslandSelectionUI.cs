using UnityEngine;
using UnityEngine.UI;

public class IndividualIslandSelectionUI : MonoBehaviour
{
    int islandIndex;
    [SerializeField]
    Image islandPreviewImage;
    [SerializeField]
    GameObject itemPreviewPrefab;
    [SerializeField]
    Transform animalPreviewParent;
    [SerializeField]
    Transform materialPreviewParent;

    public void InitUI(IslandSO isla, int index, AllObjectTypesSO bd)
    {
        islandIndex = index;
        islandPreviewImage.sprite = isla.previewInfo.previewImage;
        if(GameFlowManager.instance.generatedIsland)
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
        //TODO

    }
    public void SelectIsland()
    {
        GameFlowManager.instance.lastSelectedIsland = islandIndex;
    }
}
