using UnityEngine;

public class AnimalGenericoApoyo : AAnimal
{
    [SerializeField]
    private StikersManager stikersManager;

    public override void PlayRunAnim()
    {
        base.PlayRunAnim();
        stikersManager.SetImage(StikersGenerales.Enfadado);
    }
}
