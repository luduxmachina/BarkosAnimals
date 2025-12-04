using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerpienteInScene : AAnimal 
{
    IGrabbable grabbable;
    [Header("----------Serpiente---------")]

    [SerializeField]
    SimpleGrabber thisGrabber;
    [Header("Stats")]
    [SerializeField]
    private float stuntTime = 2.0f;
    [SerializeField]
    private float tiempoDescanso = 5.0f;
    [SerializeField]
    private float tiempoTrasAtacar = 0.75f;
    [SerializeField]
    private StikersManager stickersManager;

    protected override void Awake()
    {
        base.Awake();
        grabbable = GetComponentInParent<IGrabbable>();
    }
    public float GetTiempoTrasAtaque()
    {
        return tiempoTrasAtacar;
    }
    public float GetTiempoDescanso()
    {
        return tiempoDescanso;
    }
    public void PlayAttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    public void PlaySurpriseAnim()
    {
        stickersManager.SetImage(StikersGenerales.Incomodo);
        animator.SetTrigger("Surprise");
    } 
    public void Descansar()
    {
        stickersManager.SetImage(StikersGenerales.Corazones);
        PlayIdleAnim();
    }
    public void AttackGrabber()
    {
        IGrabber grabber = grabbable.currentGrabber;
        if(grabber != null)
        {
            grabber.gameObject.GetComponentInChildren<PlayerInSceneEffects>()?.AddStunt(stuntTime);
            PlayAttackAnim();
        }
        grabbable.Drop(); //se libera  si misma
        stickersManager.SetImage(StikersGenerales.Incomodo);
    }
    private bool CheckCart(List<ItemNames> posiblesPresas)
    {
        //poner aqui el carro si tiene animal dentro
        Transform carroT = IslandPositions.instance.GetClosest(transform.position, ItemNames.Cart);
        if(carroT == null) { return false;  }

        CartData cartData = carroT?.GetComponentInChildren<CartData>();
        if (cartData == null) { return false; }
        var temp= cartData.GetAllInventoryObjects();
        foreach (var item in temp)
        {
            if (posiblesPresas.Contains(item.Name))
            {
                return true;
            }
           
        }
        return false;

    }
    public override void InitComer()
    {
        base.InitComer();
        thisGrabber.TryGrab(lastObjectve);
        stickersManager.SetImage(StikersGenerales.NecesitaComerCarne);

    }
    public void ComerEnCarro()
    {
        CartData cartData = lastObjectve.GetComponentInChildren<CartData>();
        if (cartData == null) { return; }

        var temp = cartData.GetAllInventoryObjects();
        ItemNames itemAComer;
        int i;
        for( i = 0; i < temp.Count; i++)
        {

            if (this.objectives.Contains(temp[i].Name))
            {

                itemAComer = temp[i].Name;
                break;
            }
        }

        //esto funciona porque justo la serpiente no tiene un grabber, lo tiene algun hijo
        var cartInScene =cartData.GetComponentInChildren<CartInScene>();
        if(cartInScene== null)
        {
            cartInScene = cartData.GetComponentInParent<CartInScene>(); //tu en algun puto lado

        }

        if (cartInScene == null) { return; }

        cartInScene.OnPlayerInteraction(this.gameObject); //"abre" el carro pero no abre la interfazx porqu no es el player, y se queda como ultimo interactor
        cartData.ExtractInventoryObjectByIndex(i); //lo "saca" pero no lo spawnea en la escena asi que es como si se lo "come"
        cartInScene.Interact(animalType, this.gameObject); //con esto se deberia meter y todo funciona 

    }
    public bool ObjectiveIsCart()
    {

        if (lastObjectve == null) { return false; }
        return lastObjectve.GetComponentInChildren<CartData>() != null;
    }
    public override Transform GetClosestObjetive()
    {
        if (CheckCart(this.objectives.ToList()))
        {
            //el carro mas cercano tiene alguna presa
            Transform carroT = IslandPositions.instance.GetClosest(transform.position, ItemNames.Cart);
            Transform objective = base.GetClosestObjetive();
            if (objective == null) { return carroT; }
            float distCarro = Vector3.Distance(transform.position, carroT.position);
            float distObjective = Vector3.Distance(transform.position, objective.position);
            if (distCarro < distObjective)
            {
                return carroT;
            }
            else
            {
                return objective;
            }


        }
        return base.GetClosestObjetive();


    }
    public override Vector3 GetNewPosition()
    {
        throw new System.NotImplementedException();
    }
}


