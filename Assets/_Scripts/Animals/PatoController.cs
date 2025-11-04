using UnityEngine;

public class PatoMovement : AAnimal
{
    [Header("------------Variables propias-----------")]
    [SerializeField]
    Vector2[] points;


    public void Update()
    {
        
    }

    protected override void Attack(Transform objetive)
    {
        throw new System.NotImplementedException();
    }

    protected override void Eat()
    {
        throw new System.NotImplementedException();
    }

    protected override void RunTo(Transform objetive)
    {
        throw new System.NotImplementedException();
    }

    protected override void ScapeFrom(Transform objetive)
    {
        throw new System.NotImplementedException();
    }
    
}
