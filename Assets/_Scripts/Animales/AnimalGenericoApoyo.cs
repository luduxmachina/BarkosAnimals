using UnityEngine;

public class AnimalGenericoApoyo : AAnimal
{
    [SerializeField]
    private StikersManager stikersManager;
    [SerializeField]
    private bool stopAnimWhenGrabbed = false;
    NavegacionBOIDsExtra navegacionBOIDs;
    protected override void Awake()
    {
        base.Awake();
        navegacionBOIDs = GetComponent<NavegacionBOIDsExtra>();
    }

    public override void PlayRunAnim()
    {
        base.PlayRunAnim();
        stikersManager.SetImage(StikersGenerales.Incomodo);
    }
    protected override void Start()
    {
        base.Start();
        var temp = GetComponent<IGrabbable>();
        if (temp != null)
        {

            temp.OnGrab.AddListener(() =>
            {
                PlayIdleAnim();
            });
        }
        DesactivarBOIDS();

    }
    public void ActivarBOIDS()
    {
      //  Debug.Log("Activar BOIDS");
        if (navegacionBOIDs != null)
            navegacionBOIDs.IsActive = true;
    }
    public void DesactivarBOIDS()
    {
       // Debug.Log("Desactivar BOIDS");

        if (navegacionBOIDs != null)
            navegacionBOIDs.IsActive = false;
    }
    
}
