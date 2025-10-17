using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParentConstraint))]
public class SimpleGrabbable : MonoBehaviour, IGrabbable
{
    public IGrabber currentGrabber = null;

    public UnityEvent OnGrab;
    public UnityEvent OnDrop;
    public bool canBeGrabbed = true;
    public bool canBeDropped = true;
    [Header("PlayerFlags")]
    public bool allowExtramoveSetWhenGrabbed = true;

    public bool isBeingGrabbed { get { return currentGrabber != null; } }
    private Rigidbody rb;
    private int originalLayer;
    [SerializeField]
    ParentConstraint parentConstraint;
    [SerializeField]
    ParentConstraint thisConstraint;
    private void Awake()
    {
        if (parentConstraint == null)
        {
            parentConstraint = transform.parent.GetComponentInParent<ParentConstraint>();
        }
        if (parentConstraint == null)
        {
            parentConstraint = gameObject.transform.parent.gameObject.AddComponent<ParentConstraint>();
        }
        thisConstraint = GetComponent<ParentConstraint>();
        parentConstraint.constraintActive = false;
        thisConstraint.constraintActive = false;
        rb = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer;
    }

    public virtual bool Grab(Transform grabbingTransform, IGrabber grabber)
    {
        //Si alguien quiere hacer comprobaciones y tal pues que lo haga heredando y eso
        if (!canBeGrabbed) return false;



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
        parentConstraint.AddSource(new ConstraintSource() { sourceTransform = grabbingTransform, weight = 1 });
        parentConstraint.constraintActive = true;
        thisConstraint.constraintActive = true;


        OnGrab?.Invoke();
        return true; //el objeto se ha cogido
    }
    public virtual bool Drop()
    {
        if (!canBeDropped) return false;
        if (!isBeingGrabbed) return false; //no se puede soltar si no se esta cogido

        OnDrop?.Invoke(); //para que el grabber siga teniendo la referencia sin null

        currentGrabber.StopGrabbing();
        currentGrabber = null;


        parentConstraint.RemoveSource(0);
        parentConstraint.constraintActive = false;
        thisConstraint.constraintActive = false;
        rb.excludeLayers = 0;
        gameObject.layer = originalLayer;


        return true; //el objeto se ha soltado
    }
}
