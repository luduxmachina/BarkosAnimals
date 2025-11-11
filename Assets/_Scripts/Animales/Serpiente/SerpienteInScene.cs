using System.Collections.Generic;
using UnityEngine;

public class SerpienteInScene : MonoBehaviour
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
    public bool PeligroAcercandose()
    {
        CalcularPeligroMasCercano();
        if (Vector3.Distance(transform.position, peligroDetectado.position) <= radioDeteccionPeligro) //que podria cachear las distancias, pero me la suda
        {
            return true;
        }
        else
        {
            return false;

        }

    }
    public bool PresaEncontrada()
    {
        CalcularPresaMasCercana();

        if (presaDetectada == null)
        {
            return false;
        }
        if (Vector3.Distance(transform.position, presaDetectada.position) <= radioDeteccionComida) //que podria cachear las distancias, pero me la suda
        {
            return true;
        }
        else
        {
            return false;

        }
    }
    public bool PresaARangoDeCaza()
    {
        return false;
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
    private void CalcularPeligroMasCercano()
    {
        peligroDetectado = IslandPositions.instance.GetPlayerPosition();

        var otroPeligro = IslandPositions.instance.GetClosest(transform.position, ItemNames.Snake); //pongamos que tambie nse ahuyenten unas a otras
        if (Vector3.Distance(transform.position, otroPeligro.position) < Vector3.Distance(transform.position, peligroDetectado.position)) //el mas cercano es el otro
        {
            peligroDetectado = otroPeligro;

        }
    }
    private void CalcularPresaMasCercana()
    {
        List<ItemNames> posiblesPresas = new List<ItemNames> { ItemNames.Duck, ItemNames.Pangolin };
        presaDetectada = IslandPositions.instance.GetClosest(transform.position, posiblesPresas.ToArray()); //no siempre hay presa 

        if (CheckCart(posiblesPresas))
        {
            var cartT = IslandPositions.instance.GetCartPosition();
            if (Vector3.Distance(transform.position, cartT.position) < Vector3.Distance(transform.position, peligroDetectado.position)) //el mas cercano es el otro
            {
                presaDetectada = cartT;

            }
        }

    }
    private bool CheckCart(List<ItemNames> posiblesPresas)
    {
        //poner aqui el carro si tiene animal dentro
        Transform carroT = IslandPositions.instance.GetCartPosition();
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
}


