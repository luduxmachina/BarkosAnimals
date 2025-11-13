using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerpienteInScene : AAnimal 
{
    [Header("----------Serpiente---------")]
    IGrabbable grabbable;
    [SerializeField]
    SimpleGrabber thisGrabber;
    [Header("Stats")]
    [SerializeField]
    private float stuntTime = 2.0f;
    [SerializeField]
    private float tiempoDescanso = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        grabbable = GetComponentInParent<IGrabbable>();
    }
    public void PlayAttackAnim()
    {
        animator.SetTrigger("Attack");
    }
    public void PlaySurpriseAnim()
    {
        animator.SetTrigger("Surprise");
    }
    public void PlayRunAnim()
    {
        animator.SetTrigger("Run");
    }    
    public void PlayWalkAnim()
    {
        animator.SetTrigger("Walk");
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
    }
    private bool CheckCart(List<ItemNames> posiblesPresas)
    {
        //poner aqui el carro si tiene animal dentro
        Transform carroT = IslandPositions.instance.GetClosest(transform.position, ItemNames.Cart);
        if(carroT == null) { return false;  }


        CartData cartData = carroT?.GetComponent<CartData>();
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

    }
    public void ComerEnCarro()
    {
        Debug.Log("COmido en carro");
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


