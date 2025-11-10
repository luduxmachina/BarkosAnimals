using UnityEngine;

public class LookingForObjetive : AAction
{
    Transform animalTransform;
    float radioDetection;
    public LookingForObjetive(AAnimal animal) : base(animal)
    {
    }

    public override void Enter()
    {
        animalTransform = this._animal.transform;
        radioDetection = this._animal.GetRadioAwareness();
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        Transform objetive = this._animal.GetClosestObjetive();
        if (Vector3.Distance(objetive.position, animalTransform.position) <radioDetection)
        {
            this._animal.NewObjetivePush();
        }
    }
}
