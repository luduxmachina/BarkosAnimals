using System.Collections.Generic;
using UnityEngine;

public class SerpienteInScene : AAnimal
{
    IGrabbable grabbable;
    [Header("Stats")]
    [SerializeField]
    private float stuntTime = 2.0f;
    [SerializeField]
    private float radioDeteccionPeligro = 10.0f;
    [SerializeField]
    private float radioDeteccionComida = 15.0f;
    [SerializeField]
    private float distanciaAtaque = 2.0f;
    [SerializeField]
    private float tiempoDescanso = 5.0f;

    [Header("Misc")]
    [SerializeField]
    Transform peligroDetectado;
    [SerializeField]
    Transform presaDetectada;
    private void Awake()
    {
        grabbable = GetComponentInParent<IGrabbable>();
    }
    private void Start()
    {
        
    }
    public void AttackGrabber()
    {
        IGrabber grabber = grabbable.currentGrabber;
        if(grabber != null)
        {
            grabber.gameObject.GetComponentInChildren<PlayerInSceneEffects>()?.AddStunt(stuntTime);
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

    public override Vector3 GetNewPosition()
    {
        throw new System.NotImplementedException();
    }
}


