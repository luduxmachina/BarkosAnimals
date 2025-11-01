using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class SimpleGrabbableWithJoint : MonoBehaviour, IGrabbable
{
    //putas cosas de interfaces lol
    UnityEvent IGrabbable.OnGrab => OnGrab;

    UnityEvent IGrabbable.OnDrop => OnDrop;

    IGrabber IGrabbable.currentGrabber => currentGrabber;

    public IGrabber currentGrabber = null;

    public UnityEvent OnGrab;
    public UnityEvent OnDrop;
    public bool canBeGrabbed = true;
    public bool canBeDropped = true;
    [Header("PlayerFlags")]
    public bool allowExtramoveSetWhenGrabbed = true;

    public bool isBeingGrabbed { get { return currentGrabber != null; } }

    [SerializeField]
    private Transform grabPoint;
    
    private Rigidbody rb;
    private int originalLayer;
    HingeJoint joint;
    ImpedeExtraMoveSetEffect impedeEffect = new ImpedeExtraMoveSetEffect();
    private void Awake()
    {

        rb = GetComponent<Rigidbody>();

        originalLayer = gameObject.layer;
    }

    public virtual bool Grab(Transform grabbingTransform, IGrabber grabber)
    {
        //Si alguien quiere hacer comprobaciones y tal pues que lo haga heredando y eso
        if (!canBeGrabbed) return false;


        joint = gameObject.AddComponent<HingeJoint>();  
        currentGrabber = grabber;

        if (!allowExtramoveSetWhenGrabbed)
        {
            PlayerInSceneEffects pm = grabber.gameObject.GetComponentInChildren<PlayerInSceneEffects>();
            if (pm != null)
            {
                pm.AddEffect(impedeEffect);
            }
        }

        rb.excludeLayers = LayerMask.GetMask("Player");
        gameObject.layer = LayerMask.NameToLayer("GrabbedObj");
        rb.useGravity = false;
        //Pegarse a punto de cogida
        Vector3 pos = grabbingTransform.position;
        Quaternion rot = grabbingTransform.rotation;
        if (grabPoint != null)
        {
            pos -= grabbingTransform.forward * grabPoint.localPosition.z + grabbingTransform.up * grabPoint.localPosition.y + grabbingTransform.right * grabPoint.localPosition.x;
            rot = rot * Quaternion.Inverse(grabPoint.localRotation);
        }

        transform.position = pos;
        transform.rotation = rot;

        joint.connectedBody = grabber.gameObject.GetComponentInParent<Rigidbody>();
        joint.connectedMassScale = 0.25f;
        joint.anchor = transform.worldToLocalMatrix.MultiplyPoint3x4(grabbingTransform.position);
        joint.axis = Vector3.right;
        joint.useSpring = true;
        joint.spring = new JointSpring()
        {
            spring = 1000f,
            damper = 100f,
            targetPosition = 0f
        };

        OnGrab?.Invoke();
        return true; //el objeto se ha cogido
    }
    public virtual bool Drop()
    {
        if (!canBeDropped) return false;
        if (!isBeingGrabbed) return false; //no se puede soltar si no se esta cogido

        OnDrop?.Invoke(); //para que el grabber siga teniendo la referencia sin null


        currentGrabber.StopGrabbing();

        if (!allowExtramoveSetWhenGrabbed)
        {
            PlayerInSceneEffects pm = currentGrabber.gameObject.GetComponentInChildren<PlayerInSceneEffects>();
            if (pm != null)
            {
                pm.RemoveEffect(impedeEffect);
            }
            else
            {
                Debug.Log("No hay player effect");
            }
        }

        currentGrabber = null;

        if(joint != null)
        {
            Destroy(joint);
        }
        rb.useGravity = true;
        rb.excludeLayers = 0;
        gameObject.layer = originalLayer;


        return true; //el objeto se ha soltado
    }
}
