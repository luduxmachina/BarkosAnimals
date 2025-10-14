using System;
using System.Collections.Generic;
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
    private Color canBePlacedColor = Color.white;
    [SerializeField]
    private Color canNotBePlacedColor = Color.yellow;
    [SerializeField]
    private Color removeColor = Color.red;
    [SerializeField]
    private GameObject removingGameObject;
    private GameObject removingGameObjectInstance;
    private List<GameObject> removingMarkers = new List<GameObject>();
    
    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);

        removingGameObjectInstance = Instantiate(removingGameObject);
        Color c = removeColor;
        c.a = 0.5f;
        removingGameObjectInstance.GetComponentInChildren<Renderer>().material = previewMaterialInstance;
        removingGameObjectInstance.GetComponentInChildren<Renderer>().material.color = c;
        removingGameObjectInstance.SetActive(false);
    }

    public void StartPreview(GameObject prefab, Vector2Int size, float orientation)
    {
        if (prefab != null)
        {
            previewObject = Instantiate(prefab);
            PreparePreview();
        }
        
        cellIndicator.SetActive(true);
        PrepareCursor(size, orientation);
    }
    
    public void StartRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one, 0f);
    }

    public void StopPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, float orientation, bool validity, float gridSize, bool removing)
    {
        // Preview
        if (previewObject != null)
        {
            MovePreview(position, orientation);
            ApplyFeedbackToPreview(validity, removing);
        }
        
        // Cursor
        MoveCursor(position, orientation, gridSize);
        ApplyFeedbackToCursor(validity, removing);
    }

    public void UpdateRemovePreview(List<Vector3> occupiedWorldPositions)
    {
        foreach (Vector3 position in occupiedWorldPositions)
        {
            GameObject go = Instantiate(removingGameObjectInstance,  position, Quaternion.identity);
            removingMarkers.Add(go);
            go.SetActive(true);
        }
    }
    
    public void EraseRemovePreview()
    {
        if (removingMarkers.Count <= 0)
            return;
        
        for (int i = 0; i < removingMarkers.Count; i++)
        {
            Destroy(removingMarkers[i]);
        }
        removingMarkers.Clear();
    }

    private void MovePreview(Vector3 position, float orientation)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
        previewObject.transform.rotation = Quaternion.Euler(0,  orientation, 0);
    }

    private void MoveCursor(Vector3 position, float orientation, float gridSize)
    {
        cellIndicator.transform.position = new Vector3(
            position.x - gridSize * 0.5f, 
            cellIndicator.transform.position.y, 
            position.z - gridSize * 0.5f
        );
        
        cellIndicator.transform.rotation = Quaternion.Euler(0,  orientation, 0);
    }

    private void ApplyFeedbackToPreview(bool validity, bool removing)
    {
        Color c = validity ? canBePlacedColor : canNotBePlacedColor;
        if (removing)
            c = removeColor;
        c.a = 0.5f;

        previewMaterialInstance.color = c;
    }
    
    private void ApplyFeedbackToCursor(bool validity, bool removing)
    {
        Color c = validity ? canBePlacedColor : canNotBePlacedColor;
        if (removing)
            c = removeColor;
        c.a = 0.5f;
        
        cellIndicator.GetComponentInChildren<Renderer>().material.color = c;
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

    private void PrepareCursor(Vector2Int size, float orientation)
    {
        if (size.x <= 0 || size.y <= 0)
            return;
        
        cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
        cellIndicator.transform.localRotation = Quaternion.Euler(0, orientation, 0);
    }

    
}
