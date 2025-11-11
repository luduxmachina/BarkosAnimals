using System;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(IMovementComponent))]
public abstract class AAnimal : MonoBehaviour
{
    [Header("---------------Importante---------------")]
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected ItemNames[] predators;
    [SerializeField]
    protected ItemNames[] objectives;

    [Header("-----------------Fase 1-----------------")]
    [Tooltip("Speed when the character is relax and in patrol state")]
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    ItemNames animalType;
    protected IMovementComponent movimiento;
    protected Vector3 lastTargetPos;

    public List<IAction> activeActions;
    protected virtual void Awake()
    {
        movimiento = GetComponent<IMovementComponent>();
    }
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
    /// Objetivo más cercano
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
    public Status AndarHaciaObjetivo()
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
            //o se ha movido o un pan mas cercano
            movimiento.SetTarget(currentObjPos);

        }
        if (movimiento.HasArrived())
        {
            movimiento.CancelMove();

            return Status.Success;
        }

        movimiento.CancelMove();

        return Status.Running;
    }


    public void Die()
    {
        GameFlowManager.instance.quotaChecker.UpdateCuote(new InventoryItemDataObjects( animalType, -1));
        Destroy(this.gameObject);
    }

    public bool PredatorClose()
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
    public bool ObjectiveClose()
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



}
