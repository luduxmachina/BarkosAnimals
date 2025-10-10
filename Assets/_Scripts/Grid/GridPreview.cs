using System;
using UnityEngine;

public class GridPreview : MonoBehaviour
{
    [SerializeField] 
    private float previewYOffset = 0.05f;

    [SerializeField] 
    private GameObject cellIndicator;
    private GameObject previewObject;
    
    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;
    
    [SerializeField]
    private Color CanBePlacedColor = Color.white;
    [SerializeField]
    private Color CanNotBePlacedColor = Color.red;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
    }

    public void StartPreview(GameObject prefab, Vector2Int size)
    {
        cellIndicator.SetActive(true);
        previewObject = Instantiate(prefab);
        PrepareCursor(size);
        PreparePreview();
    }

    public void StopPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity, float gridSize)
    {
        MovePreview(position);
        MoveCursor(position, gridSize);
        ApplyFeedback(validity);
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    private void MoveCursor(Vector3 position, float gridSize)
    {
        cellIndicator.transform.position = new Vector3(
            position.x - gridSize * 0.5f, 
            cellIndicator.transform.position.y, 
            position.z - gridSize * 0.5f
        );
    }

    private void ApplyFeedback(bool validity)
    {
        previewMaterialInstance.color = validity ? CanBePlacedColor : CanNotBePlacedColor;
        cellIndicator.GetComponentInChildren<Renderer>().material.color = validity ? CanBePlacedColor : CanNotBePlacedColor;
    }

    private void PreparePreview()
    {
        Renderer[]  renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            Material[] materials = r.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            r.materials = materials;
        }
        
        var layer = LayerMask.NameToLayer("Ignore Raycast");
        previewObject.layer = layer;
        foreach (Transform child in previewObject.transform)
        {
            child.gameObject.layer = layer;
        }
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x <= 0 || size.y <= 0)
            return;
        
        cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
        // cellIndicator.GetComponentInChildren<Renderer>().material.mainTextureScale = size;
    }
}
