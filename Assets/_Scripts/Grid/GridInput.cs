using System;
using UnityEngine;

public class GridInput : MonoBehaviour
{
    public event Action OnClick, onExit;
    
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private Camera mainCamera;
    
    private Vector3 lastMousePos;

    private int lastPlacedObjectId = -1;
    private float rotation = 0f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        //////////////////// TESTEO ////////////////////
        if(Input.GetMouseButtonDown(0))
            OnClick?.Invoke();
        
        if(Input.GetKeyDown(KeyCode.Escape))
            onExit?.Invoke();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            lastPlacedObjectId = 0;
            GetComponent<GridPlacementManager>().StartPlacement(lastPlacedObjectId);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            lastPlacedObjectId = 1;
            GetComponent<GridPlacementManager>().StartPlacement(lastPlacedObjectId);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            lastPlacedObjectId = 2;
            GetComponent<GridPlacementManager>().StartPlacement(lastPlacedObjectId);
        }

        if (Input.GetKeyDown(KeyCode.A))
            GetComponent<GridPlacementManager>().SetRotation(lastPlacedObjectId, AddToRotation(-90f));
        if (Input.GetKeyDown(KeyCode.D))
            GetComponent<GridPlacementManager>().SetRotation(lastPlacedObjectId, AddToRotation(90f));

        if (Input.GetKeyDown(KeyCode.R))
        {
            lastPlacedObjectId = -1;
            GetComponent<GridPlacementManager>().StartRemoving();
        }
            
            
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.nearClipPlane;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        {
            lastMousePos = hit.point;
        }
        
        return lastMousePos;
    }

    private float AddToRotation(float angle)
    {
        float newRotation = rotation +  angle;

        if (newRotation > 180)
            newRotation -= 360;
        
        if(newRotation <= -180)
            newRotation += 360;
        
        Debug.Log($"Rotation: {newRotation}");
        rotation =  newRotation;
        return newRotation;
    }
}

