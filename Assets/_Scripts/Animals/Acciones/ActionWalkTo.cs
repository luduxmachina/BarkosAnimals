using UnityEngine;

public class ActionWalkTo : AAction
{
    private Transform animalTransform;
    private Transform objetive;
    private float speed;
    private float rotateSpeed;
    private float interactRadio;

    public ActionWalkTo(AAnimal animal) : base(animal)
    {
    }

    public override void Enter()
    {
        animalTransform  = this._animal.transform;
        speed = this._animal.GetWalkingSpeed();
        rotateSpeed = this._animal.GetRotateSpeed();
        objetive = this._animal.GetClosestObjetive();
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public override void Update()
    {
        Vector3 direccion = objetive.position - animalTransform.position;
        Quaternion rotation = Quaternion.LookRotation(direccion);
        animalTransform.rotation = Quaternion.Slerp(animalTransform.rotation, rotation, rotateSpeed * Time.deltaTime);
        this.animalTransform.LookAt(objetive);

        this.animalTransform.position += this.animalTransform.forward * this.speed * Time.deltaTime;

        if (Vector3.Distance(animalTransform.position, objetive.position)<= interactRadio)
        {
            
        }
    }
}
