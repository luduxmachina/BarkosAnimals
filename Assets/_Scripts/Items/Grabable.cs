using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
[RequireComponent(typeof(ParentConstraint))]
public class Grabable : MonoBehaviour
{

    public ItemInteraction itemInteraction;
    ParentConstraint parentConstraint;
    public UnityEvent OnGrab;
    public UnityEvent OnDrop;
    public bool canBeGrabbed = true;
    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
        parentConstraint.constraintActive = false;
    }
    public virtual bool Grab(Transform grabbingTransform)
    {
        //Si alguien quiere hacer comprobaciones y tal pues que lo haga heredando y eso
        if(!canBeGrabbed) return false; 

        Debug.Log("Grabable.Grab()");

        transform.position = grabbingTransform.position;
       // transform.rotation = grabbingTransform.rotation;

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
        OnDrop?.Invoke();

        return true; //el objeto se ha soltado
    }
}
