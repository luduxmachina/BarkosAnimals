using UnityEngine;
public abstract class AAction 
{
    protected AAnimal _animal;

    public AAction(AAnimal animal)
    {
        _animal = animal;
    }


    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
    
}
