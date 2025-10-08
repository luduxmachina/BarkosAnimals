using UnityEngine;

public class GridInput : MonoBehaviour
{
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
    
    // test
    public bool GetPlacementInput()
        => Input.GetMouseButtonDown(0);
}

