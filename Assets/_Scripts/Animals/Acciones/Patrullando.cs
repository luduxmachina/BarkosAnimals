using UnityEngine;

public class Patrullando: AAction
{
    private WalkToNextPatrollPoint walk;

    public Patrullando(AAnimal animal): base(animal)
    {
        walk = new WalkToNextPatrollPoint(animal);
    }

    public override void Enter()
    {
        walk.Enter();
    }

    public override void Update()
    {
        walk.Update();
        
    }

    public override void FixedUpdate()
    {
        walk.FixedUpdate();
    }

    public override void Exit()
    {
        walk.Exit();
    }
}
