using UnityEngine;

public class AnimalGenericoApoyo : AAnimal
{
    [SerializeField]
    private StikersManager stikersManager;
    [SerializeField]
    private bool stopAnimWhenGrabbed = false;
    public override void PlayRunAnim()
    {
        base.PlayRunAnim();
        stikersManager.SetImage(StikersGenerales.Enfadado);
    }
    protected override void Start()
    {
        base.Start();
        if (stopAnimWhenGrabbed)
        {
            var temp = GetComponent<IGrabbable>();
            if (temp != null)
            {
                temp.OnGrab.AddListener(() =>
                {
                    PlayIdleAnim();
                });
            }
        }
    }
    
}
