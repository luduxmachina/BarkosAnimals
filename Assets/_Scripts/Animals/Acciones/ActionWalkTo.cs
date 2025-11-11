using UnityEngine;
using UnityEngine.AI;

public class ActionWalkTo : AAction
{
    private Transform animalTransform;
    private Transform objetiveTransform;
    //private float speed;
    //private float rotateSpeed;
    private float interactRadio;
    //Quaternion rotation;
    NavMeshAgent agent;

    public ActionWalkTo(AAnimal animal) : base(animal)
    {
    }

    public override void Enter()
    {
        animalTransform  = this._animal.transform;
        //speed = this._animal.GetWalkingSpeed();
        //rotateSpeed = this._animal.GetRotateSpeed();
        objetiveTransform = this._animal.GetClosestObjetive();
        //Vector3 direccion = objetiveTransform.position - animalTransform.position;
        //rotation = Quaternion.LookRotation(direccion);
        agent = _animal.GetComponent<NavMeshAgent>();
        agent.SetDestination(objetiveTransform.position);

        
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        //animalTransform.rotation = Quaternion.Slerp(animalTransform.rotation, rotation, rotateSpeed * Time.deltaTime);
        //this.animalTransform.LookAt(objetiveTransform);
        //
        //this.animalTransform.position += this.animalTransform.forward * this.speed * Time.deltaTime;
        agent.SetDestination(objetiveTransform.position);

        if (Vector3.Distance(animalTransform.position, objetiveTransform.position) <= interactRadio)
        {
            //this._animal.ReachedObjetivePush();
        }
    }

    public override void Update()
    {
    }
}
