using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
[RequireComponent(typeof(ParentConstraint))]
[RequireComponent(typeof(Rigidbody))]
public class SimpleGrabbable : MonoBehaviour, IGrabbable
{
    private IGrabber currentGrabber = null;

    public UnityEvent OnGrab;
    public UnityEvent OnDrop;
    public bool canBeGrabbed = true;
    public bool canBeDropped = true;
    [Header("PlayerFlags")]
    public bool allowExtramoveSetWhenGrabbed = true;

    public bool isBeingGrabbed { get { return currentGrabber != null; } }
    private Rigidbody rb;
    private int originalLayer;
    ParentConstraint parentConstraint;
    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
        parentConstraint.constraintActive = false;
        rb = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer;
    }
    public virtual bool Grab(Transform grabbingTransform, IGrabber grabber)
    {
        //Si alguien quiere hacer comprobaciones y tal pues que lo haga heredando y eso
        if(!canBeGrabbed) return false; 

 

        currentGrabber = grabber;

        if (!allowExtramoveSetWhenGrabbed)
        {
            PlayerMovement pm = grabber.gameObject.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                pm.ImpedeExtraMoveset();
            }
        }

        rb.excludeLayers = LayerMask.GetMask("Player");
        gameObject.layer = LayerMask.NameToLayer("GrabbedObj");

        //Pegarse a punto de cogida
        transform.position = grabbingTransform.position;
        parentConstraint.AddSource( new ConstraintSource() { sourceTransform = grabbingTransform, weight = 1 });
        parentConstraint.constraintActive = true;


        OnGrab?.Invoke();
        return true; //el objeto se ha cogido
    }
    public virtual bool Drop()
    {
        if (!canBeDropped) return false;
        if(!isBeingGrabbed) return false; //no se puede soltar si no se esta cogido
        currentGrabber.StopGrabbing();
        currentGrabber = null;

        Debug.Log("Grabable.Drop()");
        parentConstraint.RemoveSource(0);
        parentConstraint.constraintActive = false;
        rb.excludeLayers = 0;
        gameObject.layer = originalLayer;
        OnDrop?.Invoke();

        return true; //el objeto se ha soltado
    }
}
