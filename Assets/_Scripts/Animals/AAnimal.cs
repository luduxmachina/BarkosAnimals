using System;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;

[RequireComponent(typeof(IMovementComponent))]
public abstract class AAnimal : MonoBehaviour
{
    [Header("---------------Importante---------------")]
    [SerializeField]
    protected Animator animator;
    [SerializeField, CustomLabel("", true)]
    protected ItemNames[] predators;
    [SerializeField, CustomLabel("", true)]
    protected ItemNames[] objectives;

    [Header("-----------------Fase 1-----------------")]
    [Tooltip("Speed when the character is relaxed and in patrol state")]
    [SerializeField]
    protected float walkingSpeed = 0.1f;


    [SerializeField]
    [Tooltip("Speed when the character is stressed or running away")]
    protected float run = 0.2f;

    [SerializeField]
    protected float rotateSpeed = 1.0f;

    [Header("-----------------Rangos-----------------")]

    [SerializeField]
    protected float radioDetectionObjective = 10f;
    [SerializeField]
    protected float radioDetectionPredator = 15f;

    [SerializeField]
    protected  float radioAtaqueComida = 2.0f;
    [Header("------------------Tiempos------------------")]
    [SerializeField]
    protected float tiempoEnComer = 3.0f;
    protected float tiempoComiendo = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected IMovementComponent movimiento;
    protected Transform lastObjectve;
    protected Vector3 lastTargetPos;

    #region Monobehaviour
    protected virtual void Awake()
    {
        movimiento = GetComponent<IMovementComponent>();
    }

    protected virtual void Start()
    {
        movimiento.Speed = walkingSpeed;
        if(movimiento as NavmeshAgentMovement)
        {
            (movimiento as NavmeshAgentMovement).minDistanceToTarget= radioAtaqueComida;
        }
    }

    protected virtual void Update(){}

    #endregion

    #region Getters
    public float GetWalkingSpeed()
    {
        return this.walkingSpeed;
    }
    public float GetRotateSpeed()
    {
        return this.rotateSpeed;
    }
    public float GetRadioAwareness()
    {
        return this.radioDetectionObjective;
    }
    public Animator GetAnimator()
    {
        return this.animator;
    }
    /// <summary>
    /// Objetivo m�s cercano
    /// </summary>
    /// <returns></returns>
    public virtual Transform GetClosestObjetive(){
        return IslandPositions.instance.GetClosest(transform.position, objectives);

    }
    /// <summary>
    /// Posiciones en plan patrulla
    /// </summary>
    /// <returns></returns>
    public abstract Vector3 GetNewPosition();
    public virtual Transform GetClosestPredator()
    {
        return IslandPositions.instance.GetClosest(transform.position, predators);
    }
    public virtual int GetNumberOfPredatorsCloser()
    {
        int count = 0;
        Transform[] allPredators = IslandPositions.instance.GetAll(predators);
        foreach (var predator in allPredators)
        {
            if (Vector3.Distance(transform.position, predator.position) <= radioDetectionPredator)
            {
                count++;
            }
        }
        return count;
    }

    #endregion

    #region Actions
    public virtual void InitComer()
    {
        if(!ObjectiveClose()) { return; }
        lastObjectve = GetClosestObjetive();
        var temp = lastObjectve.GetComponentInChildren<ItemInScene>();
        if (temp) //se lo va a comer lit
        {
            if (animator)
            {
                animator.SetTrigger("Comer");

            }
        }
        tiempoComiendo = 0.0f;

        //  if (!) { return;  }
        //animator supongo
        //comer el pan
        //  comidaObjetivo.GetComponentInChildren<ItemInScene>()?.ReduceByOne();
    }
    public virtual Status UpdateComer()
    {
        if (!ObjectiveClose()) //la comida puede desaparecer
        {
            if (animator)
            {
                animator.SetTrigger("Idle");

            }
            tiempoComiendo = 0.0f;

            return Status.Failure;
        }
        if(Vector3.Distance(transform.position, lastObjectve.position) > radioAtaqueComida*1.25) //alguien ha movido la comida o al animal y ya no esta comiendo lol
        {
            if (animator)
            {
                animator.SetTrigger("Idle");

            }
            tiempoComiendo = 0.0f;

            return Status.Failure;
        }

        tiempoComiendo += Time.deltaTime;
        if(tiempoComiendo >= tiempoEnComer)
        {
            var temp = lastObjectve.GetComponentInChildren<ItemInScene>();
            if (temp) //se lo va a comer lit
            {
                temp.ReduceByOne();
            }
            if (animator)
            {
                animator.SetTrigger("Idle");

            }
            tiempoComiendo = 0.0f;
            return Status.Success;
        }
        return Status.Running;
    }
    public virtual Status MoveTowardsObjective()
    {
        if (!ObjectiveClose()) //la comida puede desaparecer
        {
            movimiento.CancelMove();

            return Status.Failure;
        }
        Transform objTR = GetClosestObjetive();
        Vector3 currentObjPos = objTR.position;
        
        if (lastTargetPos != currentObjPos)
        {
            lastTargetPos= currentObjPos;
            lastObjectve = objTR;
            //o se ha movido o un pan mas cercano
            movimiento.SetTarget(currentObjPos);

        }
        if (movimiento.HasArrived())
        {
            movimiento.CancelMove();

            return Status.Success;
        }


        return Status.Running;
    }


    public virtual void Die()
    {
        //no creo que este bien 
        Destroy(this.gameObject);
    }

    public virtual bool NotObjectiveCloseToAttack()
    {
        return !this.ObjectiveCloseToAttack();
    }

    #endregion

    #region Pulls

    public virtual bool PredatorClose()
    {
        if(GetClosestPredator() != null && Vector3.Distance(transform.position, GetClosestPredator().position) <= radioDetectionPredator)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual bool NotPredatorClose()
    {
        return !this.PredatorClose();
    }
    public virtual bool ObjectiveClose()
    {
        if (GetClosestObjetive() != null && Vector3.Distance(transform.position, GetClosestObjetive().position) <= radioDetectionObjective)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public virtual bool NotObjectiveClose()
    {
        return !this.ObjectiveClose();
    }
    public virtual bool ObjectiveCloseToAttack()
    {
        if(GetClosestObjetive() != null && Vector3.Distance(transform.position, GetClosestObjetive().position) <= radioAtaqueComida)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}


