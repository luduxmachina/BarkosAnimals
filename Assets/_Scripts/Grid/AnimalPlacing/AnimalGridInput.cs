using System;
using UnityEngine;

public class AnimalGridInput : MonoBehaviour, IGridInput
{
    public event Action OnClick;
    public event Action OnExit;
    
    [SerializeField]
    private LayerMask animalGroundLayerMask;
    [SerializeField]
    private Camera mainCamera;

    [SerializeField] private float yOffset = 0.5f;
    
    private GridPlacementManager gridPlacementManager;
    private Vector3 lastMousePos;
    
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        lastMousePos = new Vector3(999f, 999f, 999f);
    }
    
    private void Start()
    {
        gridPlacementManager = GetComponent<GridPlacementManager>();
    }
    
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.nearClipPlane;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100, animalGroundLayerMask))
            return;
        
        //////////////////// TESTEO ////////////////////
        if (Input.GetMouseButtonUp(0))
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
        if (Physics.Raycast(ray, out hit, 100, animalGroundLayerMask))
        {
            lastMousePos = hit.point;
        }
        lastMousePos = new Vector3(lastMousePos.x, lastMousePos.y + yOffset, lastMousePos.z);

        return lastMousePos;
    }

    public void StartPlacing(int id)
    {
        gridPlacementManager.StartPlacement(id);
    }

    public void StopPlacing()
    {
        OnExit?.Invoke();
    }

    public void StartRemoving()
    {
        gridPlacementManager.StartRemoving();
    }
}
