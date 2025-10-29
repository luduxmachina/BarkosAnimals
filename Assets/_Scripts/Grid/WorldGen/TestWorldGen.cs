using UnityEngine;

public class TestWorldGen : MonoBehaviour
{
    [SerializeField]
    private WorldPlaceableObjectsSO database;

    private void Start()
    {
        foreach(var obj in database.worldObjectData)
        {
            Debug.Log($"la matriz es de {obj.B_OcupiedSpace.GetColums()} x {obj.B_OcupiedSpace.GetRows()}");
        }
    }
}
