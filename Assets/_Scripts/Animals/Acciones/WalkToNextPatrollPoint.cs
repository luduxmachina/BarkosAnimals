using UnityEngine;
using UnityEngine.AI;

public class WalkToNextPatrollPoint : AAction
{

    private Transform animalTransform;
    private Vector3 objetivePosition;
    private float speed;
    private float rotateSpeed;
    private float interactRadio;
    Vector3 newPos;
    Quaternion rotation;
    NavMeshAgent agent;


    public WalkToNextPatrollPoint(AAnimal animal) : base(animal)
    {
    }

    public override void Enter()
    {
        animalTransform = this._animal.transform;
        agent = this._animal.GetComponent<NavMeshAgent>();
       // speed = this._animal.GetWalkingSpeed();
       // rotateSpeed = this._animal.GetRotateSpeed();
        newPos = _animal.GetNewPosition();
        agent.SetDestination(newPos);
        //Vector3 direccion = newPos - animalTransform.position;
        //rotation = Quaternion.LookRotation(direccion);

    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdate()
    {
        //animalTransform.rotation = Quaternion.Slerp(animalTransform.rotation, rotation, rotateSpeed * Time.deltaTime);
        //
        //this.animalTransform.position += this.animalTransform.forward * this.speed * Time.deltaTime;

        if (Vector3.Distance(animalTransform.position, newPos) <= interactRadio)
        {
            this._animal.ReachedObjetivePush();
        }
    }

    public override void Update()
    {
    }
}