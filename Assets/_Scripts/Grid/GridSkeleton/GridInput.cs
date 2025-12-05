using System;
using UnityEngine;

public class GridInput : MonoBehaviour, IGridInput
{
    public event Action OnClick, OnExit;

    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private Camera mainCamera;

    private GridPlacementManager gridPlacementManager;
    private Vector3 lastMousePos;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Start()
    {
        gridPlacementManager = GetComponent<GridPlacementManager>();
    }
    
    public void StopPlacing()
    {
        OnExit?.Invoke();
    }

    public void StartPlacing(int id)
    {
        gridPlacementManager.StartPlacement(id);
    }

    public void StartRemoving()
    {
        gridPlacementManager.StartRemoving();
    }

    private void Update()
    {
        //////////////////// TESTEO ////////////////////
        if (Input.GetMouseButtonDown(0))
            OnClick?.Invoke();

        // if (Input.GetKeyDown(KeyCode.Escape))
        //     StopPlacing();
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
}

