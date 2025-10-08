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
        
        if(Input.GetKeyDown(KeyCode.Q))
            GetComponent<GridPlacementManager>().StartPlacement(0);
        if(Input.GetKeyDown(KeyCode.W))
            GetComponent<GridPlacementManager>().StartPlacement(1);
        if(Input.GetKeyDown(KeyCode.E))
            GetComponent<GridPlacementManager>().StartPlacement(2);
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
    
    //////////////////// TESTEO ////////////////////
    public bool GetPlacementInput()
        => Input.GetMouseButtonDown(0);
}

