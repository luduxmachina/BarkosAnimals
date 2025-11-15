using UnityEngine;

public class AplicarSinDelPatoEspagueti : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;
    void Start()
    {
        Material mat = CurrentSkin.currentSkin.skinMaterial;       
        if(mat != null)
        {
            skinnedMeshRenderer.material = mat;
        }
    }

}
