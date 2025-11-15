using UnityEngine;
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteraction : MonoBehaviour, IGrabber
{
    [SerializeField] CompleteTaggedDetector detector;
    [SerializeField] Transform posCogerObj;

    [SerializeField, ReadOnly] bool hasObjInHand = false;

    [SerializeField] private Animator animator;

    [SerializeField, ReadOnly, HideIf("hasObjInHand", false)]
    IGrabbable objInHand;

    IContinuousPlayerInteractionReciever continuousTarget;
    public void StopInteractingWithTarget()
    {
        if (continuousTarget == null) { return; }
       continuousTarget.OnPlayerStopInteraction(gameObject);
        continuousTarget = null;
    }
    public void Interact()
    {
        bool interacted = false;
        if (!hasObjInHand )
        {
            if (detector.HasTarget())
            {
               interacted= InteractWith(detector.GetTarget()); 
            }
        }
        else
        {
            interacted = InteractWith(objInHand.gameObject);

        }

        //si quereis hacer un sonidito o algo con interacted aqui:


    }
    private bool InteractWith(GameObject target)
    {
        IPlayerInteractionReciever interactable = target.GetComponent<IPlayerInteractionReciever>();
        bool interacted = false;
        if (interactable != null)
        {
    
             interacted = interactable.OnPlayerInteraction(gameObject);


            
        }

        IContinuousPlayerInteractionReciever continuousInteractable = target.GetComponent<IContinuousPlayerInteractionReciever>();
        if (continuousInteractable != null)
        {
            if(continuousTarget != null && continuousTarget != continuousInteractable)
            {
                StopInteractingWithTarget();
            }
            continuousTarget = continuousInteractable;
            interacted = interacted || continuousInteractable.OnPlayerStartInteraction(gameObject);
        }
        return interacted;
    }
    public void Grab()
    {
        if(hasObjInHand) { 
            DropObj();
            return;
        }

        if (!detector.HasTarget()) { return; } //no ocurre nada

        GameObject target = detector.GetTarget();
        IGrabbable grabable = target.GetComponent<IGrabbable>();
        if (grabable != null)
        {
            GrabObject(grabable);

            return; //se ha cogido el objeto
        }
        return; //no se ha cogido el objeto
    }
    public void GrabObject(IGrabbable grabbable)
    {
        if (hasObjInHand) { DropObj(); return; } //si ya tengo algo en la mano, lo suelto primero

        if (grabbable.Grab(posCogerObj, this))
        {

            objInHand = grabbable;
            hasObjInHand = true;
            animator.SetTrigger("Take");
            animator.SetBool("TakeSomething", true);
            return; //se ha cogido el objeto
        }
    }
    public void DropObj() //lo dejo public porque algo puede hacerme soltar el objeto
    {
        if (!hasObjInHand) { return; }

        //soltar objeto
        objInHand.Drop(); //esto, si puedo soltarlo, me llamaran a StopGrabbing
 
        return;
    }

    public void StopGrabbing()
    {

        hasObjInHand = false;
        objInHand = null;
    }
}
