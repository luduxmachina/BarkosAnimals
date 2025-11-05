using UnityEngine;

public class PlaceableObjectDataBase : IPlaceableObjectData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public CustomBoolMatrix OcupiedSpace { get; private set; } //= new CustomBoolMatrix();

    public Vector2Int Size
    {
        get
        {
            if (OcupiedSpace == null)
                return Vector2Int.zero;

            return new Vector2Int(OcupiedSpace.rows, OcupiedSpace.columns);
        }
    }
}