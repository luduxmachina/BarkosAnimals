using UnityEngine;

public class PatoMovement : AAnimal
{
    [Header("------------Variables propias-----------")]
    [SerializeField]
    Vector2[] points;

    public override Transform GetClosestObjetive()
    {
        throw new System.NotImplementedException();
    }

    public override Transform GetClosestPredator()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 GetNewPosition()
    {
        throw new System.NotImplementedException();
    }

    public override int GetNumberOfPredatorsCloser()
    {
        throw new System.NotImplementedException();
    }


    
}
