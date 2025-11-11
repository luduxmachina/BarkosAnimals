using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    public void IrAEstanqueInit()
    {

        if (posEstanque != null || !movimiento.CanMove(posEstanque.position))
        {
            movimiento.SetTarget(posEstanque.position);
            if (animator != null)
            {
                animator.SetFloat("Speed", walkingSpeed);

            }
        }
    }
    public Status IrAEstanqueUpdate()
    {

        if (movimiento.HasArrived())
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", 0);

            }

            return Status.Success;
           
        }
        if(Vector3.Distance(transform.position, posEstanque.position) <= radioDentroEstanque)
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", 0);

            }

            return Status.Success;
        }
        if (!movimiento.CanMove(posEstanque.position))
        {

            if (animator != null)
            {
                animator.SetFloat("Speed", 0);

            }

            return Status.Failure;
        }

        return Status.Running;
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
  


    public override Vector3 GetNewPosition()
    {
        throw new System.NotImplementedException();
    }
}
