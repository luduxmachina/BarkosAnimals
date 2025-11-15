using UnityEngine;

public class CannonItem : MonoBehaviour
{
    [SerializeField] private SimpleGrabbableWithJoint grabbable;
    [SerializeField]
    Rigidbody thisRb;
    Rigidbody empujeRB;
    [SerializeField]
    private CannonDisparador disparador;
    public float retroceso = 700f;
    private void Start()
    {
        thisRb = GetComponent<Rigidbody>();
    }
    public void Fire()
    {
        //poner el del grabbr si esta cogida
       empujeRB.AddForce(-transform.forward * retroceso * empujeRB.mass);
        Debug.Log("EMpuje a " + empujeRB.gameObject.name);
        disparador.Fire();
    }
    private void OnEnable()
    {
        grabbable.OnGrab.AddListener(OnGrabbed);
        grabbable.OnDrop.AddListener(OnReleased);
    }
    private void OnDisable()
    {
        grabbable.OnGrab.RemoveListener(OnGrabbed);
        grabbable.OnDrop.RemoveListener(OnReleased);
    }
    private void OnGrabbed()
    {
        empujeRB = grabbable.currentGrabber.gameObject.GetComponent<Rigidbody>();
    }
    private void OnReleased()
    {
        empujeRB = thisRb;
    }
}


