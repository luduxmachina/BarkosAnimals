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
    [SerializeField]
    float retroceso = 30.0f;
    [SerializeField]
    float retrocesoInicial = 150.0f;
    [SerializeField]
    float rotacionRandom= 5.0f;
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

        empujeRB.AddForce(empujeRB.mass * retrocesoInicial * -transform.forward);
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

       empujeRB.AddForce(empujeRB.mass * retroceso * -transform.forward);
        float dir = 0;
        if(Time.fixedTime % 0.5f < 0.25f)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        empujeRB.AddTorque(empujeRB.mass * rotacionRandom * dir * transform.up);
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
        grabbable?.OnGrab?.RemoveListener(OnGrabbed);
        grabbable?.OnDrop?.RemoveListener(OnReleased);
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


