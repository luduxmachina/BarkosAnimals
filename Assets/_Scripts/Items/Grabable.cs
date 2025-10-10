using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
[RequireComponent(typeof(ParentConstraint))]
[RequireComponent(typeof(Rigidbody))]
public class Grabable : MonoBehaviour
{

    public ItemInteraction itemInteraction;
    ParentConstraint parentConstraint;
    public UnityEvent OnGrab;
    public UnityEvent OnDrop;
    public bool canBeGrabbed = true;
    private Rigidbody rb;
    private int originalLayer;
    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
        parentConstraint.constraintActive = false;
        rb = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer;
    }
    public virtual bool Grab(Transform grabbingTransform)
    {
        //Si alguien quiere hacer comprobaciones y tal pues que lo haga heredando y eso
        if(!canBeGrabbed) return false; 

        Debug.Log("Grabable.Grab()");

        
        rb.excludeLayers = LayerMask.GetMask("Player");
        gameObject.layer = LayerMask.NameToLayer("GrabbedObj");
        transform.position = grabbingTransform.position;
        parentConstraint.AddSource( new ConstraintSource() { sourceTransform = grabbingTransform, weight = 1 });
        parentConstraint.constraintActive = true;
        OnGrab?.Invoke();
        return true; //el objeto se ha cogido
    }
    public virtual bool Drop()
    {
        Debug.Log("Grabable.Drop()");
        parentConstraint.RemoveSource(0);
        parentConstraint.constraintActive = false;
        rb.excludeLayers = 0;
        gameObject.layer = originalLayer;
        OnDrop?.Invoke();

        return true; //el objeto se ha soltado
    }
}
