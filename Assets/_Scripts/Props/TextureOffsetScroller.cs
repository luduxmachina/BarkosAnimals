using UnityEngine;

public class TextureOffsetScroller : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.1f, 0f);
    [SerializeField] private string textureName = "_BaseMap"; 

    private Renderer rend;
    private Material targetMat; 
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend.materials.Length > 1)
        {
            // Copiaaaa del materia
            targetMat = new Material(rend.materials[1]);
            var mats = rend.materials;
            mats[1] = targetMat;
            rend.materials = mats;

            
            offset = targetMat.GetTextureOffset(textureName);
        }
        else
        {
            Debug.LogWarning($"{name} no tiene un segundo material para desplazar la textura.");
        }
    }

    void Update()
    {
        if (player != null && targetMat != null && player.moveInput.sqrMagnitude > 0.1f)
        {
            offset += scrollSpeed * Time.deltaTime;
            targetMat.SetTextureOffset(textureName, offset);
        }
    }
}
