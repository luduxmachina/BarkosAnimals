using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatoFase1 : MonoBehaviour
{
    [SerializeField]
    NavmeshAgentMovement movimiento;

    [Header("Datos/Stats")]
    public float radioDeteccionComida;
    public List<Transform> puntosEstanque;
    [SerializeField]
    private int energiaMax = 6;
    [Header("Misc")]
    [SerializeField, Tooltip("La distancia a la que el pato considera que está pegado al pan para comérselo")]
    private float radioAlcance;
    [SerializeField, Tooltip("Desde e lcentro del estanque cuanto 'es agua'")]
    private float radioDentroEstanque;
    [SerializeField]
    private int energiaActual=6;
    public Transform posEstanque;
    [SerializeField, ReadOnly]
    private Transform comidaObjetivo;
    private Vector3 comidaObjetivoPos;
    private void Start()
    {
        energiaActual = energiaMax;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool HayPan()
    {
        if (IslandPositions.instance)
        {
            var temp= IslandPositions.instance.HasType(ItemNames.Bread);
            if(temp)
            {
                return Vector3.Distance(transform.position, IslandPositions.instance.GetClosest(transform.position, ItemNames.Bread).position) <= radioDeteccionComida;
            }
        }
        return false;
    }
    public void FijarObjetivo()
    {
        comidaObjetivo = IslandPositions.instance.GetClosest(transform.position, ItemNames.Bread);
        comidaObjetivoPos = comidaObjetivo.position;
    }
   
    public bool EnAgua()
    {
        if (posEstanque)
        {
            return Vector3.Distance(transform.position, posEstanque.position) <= radioDentroEstanque;
        }
        return false;
    }
    public bool FueraAgua()
    {
        return !EnAgua();
    }
    public bool EstaCansado()
    {
        return energiaActual <= 0.0f;
    }
    public void UsarEnergia() 
    {
        energiaActual--;

    }
    public void Descansar()
    {
        energiaActual = energiaMax;
        //Animator supongo
        Debug.Log("Descansando");

    }
    public void Aletear()
    {
        //animator supongo
        Debug.Log("Aleteando");
    }
    public void Comer()
    {
        if (!comidaObjetivo) { return;  }
        //animator supongo
        //comer el pan
        comidaObjetivo.GetComponentInChildren<ItemInScene>()?.ReduceByOne();
    }
    public Status AndarHaciaComida()
    {
        if (!HayPan()) //la comida puede desaparecer
        {
            return Status.Failure;
        }
        Vector3 lastTargetPos = comidaObjetivoPos;
        FijarObjetivo(); //puede que haya otro pan mas cerca 
        if(lastTargetPos != comidaObjetivoPos)
        {
            //o se ha movido o un pan mas cercano
            movimiento.SetTarget(comidaObjetivo.position);

        }
        if (movimiento.HasArrived())
        {
            return Status.Success;
        }

        return Status.Running;
    }
    public void StopAndarHaciaComida()
    {
        movimiento.CancelMove();
    }
}
