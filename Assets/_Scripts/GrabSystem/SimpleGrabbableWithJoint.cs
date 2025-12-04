using BehaviourAPI.UnityToolkit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SimpleGrabbableWithJoint : MonoBehaviour, IGrabbable
{
    [Header("Disable settings")]
    [SerializeField] bool disableNavMeshAgentOnGrab = true;
    [SerializeField] bool disableBehavioursOnGrab = true;
    [SerializeField] PhysicsMaterial grabbedMaterial;
    PhysicsMaterial originalMaterial;
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
    private void Start()
    {
        originalMaterial = GetComponent<Collider>().material;
    }
    public virtual bool Grab(Transform grabbingTransform, IGrabber grabber)
    {
        //Si alguien quiere hacer comprobaciones y tal pues que lo haga heredando y eso
        if (!canBeGrabbed) return false;
        if (isBeingGrabbed)
        {
            currentGrabber.StopGrabbing();
        }

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

        //rb.excludeLayers = LayerMask.GetMask("Player");
        gameObject.layer = LayerMask.NameToLayer("GrabbedObj");


        rb.useGravity = false;
        //Pegarse al grabberTransform
        Vector3 pos = grabbingTransform.position;
        Quaternion rot = grabbingTransform.rotation;
        if (grabPoint != null)
        {
            pos -= grabbingTransform.forward * grabPoint.localPosition.z + grabbingTransform.up * grabPoint.localPosition.y + grabbingTransform.right * grabPoint.localPosition.x;
            rot = rot * Quaternion.Inverse(grabPoint.localRotation);
        }

        transform.position = pos;
        transform.rotation = rot;

        joint.connectedBody = grabbingTransform.gameObject.GetComponentInChildren<Rigidbody>();
        if(joint.connectedBody == null)
        {
            Debug.LogWarning("En algun lado habrá rigid body lol");
            grabber.gameObject.GetComponentInParent<Rigidbody>();
        }
        joint.anchor = transform.worldToLocalMatrix.MultiplyPoint3x4(grabbingTransform.position);
        joint.axis = Vector3.right;
        joint.useSpring = true;
        joint.spring = new JointSpring()
        {
            spring = 2000f,
            damper = 500f,
            targetPosition = 0f
        };

        GetComponent<Collider>().material = grabbedMaterial;


        OnGrab?.Invoke();
        if (disableNavMeshAgentOnGrab)
        {
            var temp = GetComponent<NavMeshAgent>();
            if (temp != null)
            {
                temp.enabled = false;
            }
        }
        if (disableBehavioursOnGrab)
        {
            var temp = GetComponent<BehaviourRunner>();
            if (temp != null)
            {
                temp.enabled = false;
            }
        }

        return true; //el objeto se ha cogido
    }
    public virtual bool Drop()
    {
        Debug.Log("Drop");

        if (!canBeDropped) return false;
        if (!isBeingGrabbed) return false; //no se puede soltar si no se esta cogido

        OnDrop?.Invoke(); //para que el grabber siga teniendo la referencia sin null

        Debug.Log("Se ha mandado la orden de soltar el objeto a " + currentGrabber.gameObject.name);
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

        GetComponent<Collider>().material = originalMaterial;

        if (disableNavMeshAgentOnGrab)
        {
            var temp = GetComponent<NavMeshAgent>();
            if (temp != null)
            {
                temp.enabled = true;
            }

        }
         if (disableBehavioursOnGrab)
        {
            var temp = GetComponent<BehaviourRunner>();
            if (temp != null)
            {
                temp.enabled = true;
            }
        }

        return true; //el objeto se ha soltado
    }
}
