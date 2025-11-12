using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UtilitySystems;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatoFase1 : AAnimal
{


    [Header("Datos/Stats")]
    public List<Transform> puntosEstanque;
    [SerializeField]
    private int energiaMax = 6;
    [Header("Misc")]
    [SerializeField, Tooltip("Desde e lcentro del estanque cuanto 'es agua'")]
    private float radioDentroEstanque;
    [SerializeField]
    private int energiaActual=6;
    public Transform posEstanque;

    private void Start()
    {
        energiaActual = energiaMax;
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
      //  if (!) { return;  }
        //animator supongo
        //comer el pan
      //  comidaObjetivo.GetComponentInChildren<ItemInScene>()?.ReduceByOne();
    }

    public override Vector3 GetNewPosition()
    {
        throw new System.NotImplementedException();
    }
    
}
