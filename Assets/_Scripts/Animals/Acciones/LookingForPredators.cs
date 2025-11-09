using UnityEngine;

public class LookingForPredators : AAction
{    
    public LookingForPredators(AAnimal animal) : base(animal){}

    float radioDetection;

    public override void Enter()
    {
        radioDetection = _animal.GetRadioAwareness();
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        Transform predator = this._animal.GetClosestPredator();
        if(Vector3.Distance(predator.position, this._animal.transform.position) < radioDetection)
        {
            _animal.PredatorClosePush();
        }
    }

    public override void Update()
    {
    }
}
