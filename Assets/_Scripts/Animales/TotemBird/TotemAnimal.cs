using BehaviourAPI.Core;
using UnityEngine;

public class TotemAnimal : AAnimal
{
    [SerializeField]
    public Transform nido;
    [SerializeField]
    SimpleGrabber grabber;
    [SerializeField]
    DetectorVision detectorVisionObjetivos;

    [SerializeField]
    DetectorVision detectorVisionPredators;
    Transform lastSeenObjective;
    GameObject grabbedObj;
    protected override void Start()
    {
        base.Start();
        detectorVisionPredators.objetivosDetectar.AddRange(this.predators);
        detectorVisionObjetivos.objetivosDetectar.AddRange(this.objectives);

    }

    public override Transform GetClosestObjetive()
    {
        if (lastSeenObjective == null)
        {
            return lastSeenObjective;
        }
        else
        {
            return detectorVisionObjetivos.transformObjetivo;
        }
    }
    public bool ObjectiveInSight()
    {
        bool objetivoVisto= detectorVisionObjetivos.hayObjetivosARango;

        if (objetivoVisto)
        {
            lastSeenObjective = detectorVisionObjetivos.transformObjetivo;

        }
        else
        {
            lastSeenObjective = null;
        }
        return objetivoVisto;
    }
    public bool PredatorInSight()
    {
        return detectorVisionPredators.hayObjetivosARango;
    }
    public bool NotPredatorInSight()
    {
        return !PredatorInSight();
    }
    public void Cogido()
    {
        movimiento.CancelMove();
        lastSeenObjective = null;
        PlayIdleAnim();
    }
    public void CogerComida()
    {
        //el grabber 
        grabber.TryGrab(detectorVisionObjetivos.transformObjetivo);
        grabbedObj = detectorVisionObjetivos.transformObjetivo.gameObject;
        //poner algun corazon o algo
        //  stickersManager.SetImage(StikersGenerales.NecesitaComerCarne);
    }

    public void LlevarComidaInit()
    {
        PlayWalkingAnim();
        movimiento.SetTarget(nido.position);
        //hacer un move towards al nido y al final dejar el pan en el nido

        //si no ha cogido  se jode

    }
    public Status LlevarComidaUpdate()
    {
        if (!grabber.hasObjInHand)
        { //le han quitado el pan o algo
            return Status.Failure;
        }
        if (movimiento.HasArrived())
        {
            //interactuar con el pan
            
            PlayIdleAnim();
            movimiento.CancelMove();
            var temp = grabbedObj.GetComponent<IPlayerInteractionReciever>();
            if (temp != null)
            {
                temp.OnPlayerInteraction(gameObject);
                if (grabber.hasObjInHand)
                {
                    grabber.DropObj(); //esto quiere decrique ha pasado de mi culo y no ha dejado el pan gucci
                }
            }
            else
            {
                grabber.DropObj(); //no se que habra cogido pero bueno
            }

            return Status.Success;
        }


        return Status.Running;
    }

}
