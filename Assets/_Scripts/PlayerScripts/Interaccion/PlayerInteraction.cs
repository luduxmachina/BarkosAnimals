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
    [Header("Audios")]
    [SerializeField] private PlayerSoundManager soundManager;

    IContinuousPlayerInteractionReciever continuousTarget;
    GameObject proxyColliders;
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
        if (interacted)
        {
            soundManager.ActivarSonidoPickUp();

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
            CopiarColliders(grabbable.gameObject);
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
       if(proxyColliders != null)
        {
            Destroy(proxyColliders);
        }
        hasObjInHand = false;
        objInHand = null;
    }
    private void CopiarColliders(GameObject grabbed)
    {

        Collider[] cols = grabbed.GetComponentsInChildren<Collider>();

        foreach (Collider col in cols)
        {
            if (col.isTrigger)
            {
                continue;
            }
            // create a container object on the player
            proxyColliders = new GameObject();
            proxyColliders.layer = LayerMask.NameToLayer("GrabbedObj");
            proxyColliders.transform.SetParent(this.transform);
            proxyColliders.transform.position = col.transform.position;
            proxyColliders.transform.rotation = col.transform.rotation;
            var temp = proxyColliders.AddComponent<FollowObject>();
            temp.target = col.transform;

            if (col is BoxCollider bc)
            {
                var copy = proxyColliders.AddComponent<BoxCollider>();
                copy.size = bc.size;
                copy.center = bc.center;
                copy.excludeLayers = LayerMask.GetMask("GrabbedObj");
            }
            else if (col is SphereCollider sc)
            {
                var copy = proxyColliders.AddComponent<SphereCollider>();
                copy.radius = sc.radius;
                copy.center = sc.center;
                copy.excludeLayers = LayerMask.GetMask("GrabbedObj");

            }
            else if (col is CapsuleCollider cc)
            {
                var copy = proxyColliders.AddComponent<CapsuleCollider>();
                copy.radius = cc.radius;
                copy.height = cc.height;
                copy.direction = cc.direction;
                copy.center = cc.center;
                copy.excludeLayers = LayerMask.GetMask("GrabbedObj");

            }

        }
    }
}
