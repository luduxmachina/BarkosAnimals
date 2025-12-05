using UnityEngine;

public class AplicarSkinPlayer : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;
    void Start()
    {
        Material mat = CurrentSkin.currentPlayerSkin.skinMaterial;
        if (mat != null)
        {

            skinnedMeshRenderer.material = mat;
        }
    }

}
