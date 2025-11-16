using System;
using UnityEngine;

public class AnimalGridInput : MonoBehaviour, IGridInput
{
    public event Action OnClick;
    public event Action OnExit;
    
    [SerializeField]
    private LayerMask animalGroundLayerMask = LayerMask.GetMask("AnimalCell");
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
    
    private void Update()
    {
        //////////////////// TESTEO ////////////////////
        if (Input.GetMouseButtonDown(0))
            OnClick?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
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

        return lastMousePos;
    }

    public void StartPlacing(int id)
    {
        gridPlacementManager.StartPlacement(id);
    }

    public void StartRemoving()
    {
        gridPlacementManager.StartRemoving();
    }
}
