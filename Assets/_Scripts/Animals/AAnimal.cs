using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AAnimal : MonoBehaviour
{
    [Header("---------------Importante---------------")]
    [SerializeField]
    protected Animator animator;


    [Header("-----------------Fase 1-----------------")]
    [Tooltip("Speed when the character is relax and in patrol state")]
    [SerializeField]
    protected float walkingSpeed = 0.1f;
    [SerializeField]
    protected float radioDetection = 10f;

    [SerializeField]
    private UnityEvent OnReachedObjetive;
    private UnityEvent OnNewObjetiveDescovered;

    [SerializeField]
    [Tooltip("Speed when the character is stressed or running away")]
    protected float run = 0.2f;

    [SerializeField]
    protected float rotateSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    QuotaChecker quotaChecker;
    ItemNames animalType;

    public List<IAction> activeActions;
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
        return this.radioDetection;
    }
    public Animator GetAnimator()
    {
        return this.animator;
    }

    public abstract Transform GetClosestObjetive();
    public abstract Vector3 GetNewPosition();
    public abstract Transform GetClosestPredator();
    public abstract int GetNumberOfPredatorsCloser();

    public void ReachedObjetivePush()
    {
        this.OnReachedObjetive.Invoke();
        //Ahora estos son invokes pero llamarán a los push en la api de los profes.
    }

    public void NewObjetivePush()
    {
        this.OnNewObjetiveDescovered.Invoke();
    }

    public void PredatorClosePush()
    {
        throw new System.NotImplementedException();
    }

}
