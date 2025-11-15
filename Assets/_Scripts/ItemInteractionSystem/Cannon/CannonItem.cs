using UnityEngine;

public class CannonItem : MonoBehaviour
{
    private IGrabbable grabbable;
    private InteractsWithPlayerConti continuousInteraction;
    [SerializeField]
    Rigidbody thisRb;
    Rigidbody empujeRB;
    [SerializeField]
    private CannonDisparador disparador;
    public float retroceso = 30.0f;
    [SerializeField, ReadOnly]
    bool firing = false;
    private void Awake()
    {
        grabbable = GetComponent<IGrabbable>();
        continuousInteraction = GetComponent<InteractsWithPlayerConti>();
    }
    private void Start()
    {
        thisRb = GetComponent<Rigidbody>();
    }
    public void StartFire()
    {
        firing = true;
    }
    public void StopFire()
    {
        firing = false;
    }
    private void FixedUpdate()
    {
        if (firing)
        {
            Fire();
        }
    }

    public void Fire()
    {
        //poner el del grabbr si esta cogida
       empujeRB.AddForce(-transform.forward * retroceso * empujeRB.mass);
     
        disparador.Fire();
    }
    private void OnEnable()
    {
        grabbable.OnGrab.AddListener(OnGrabbed);
        grabbable.OnDrop.AddListener(OnReleased);
        continuousInteraction.OnPlayerInteractStart.AddListener(StartFire);
        continuousInteraction.OnPlayerInteractStop.AddListener(StopFire);
    }
    private void OnDisable()
    {
        grabbable.OnGrab.RemoveListener(OnGrabbed);
        grabbable.OnDrop.RemoveListener(OnReleased);
        continuousInteraction.OnPlayerInteractStart.RemoveListener(StartFire);
        continuousInteraction.OnPlayerInteractStop.RemoveListener(StopFire);
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


