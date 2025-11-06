using System;
using UnityEngine;

public abstract class AAnimal : MonoBehaviour
{
    [Header("---------------Importante---------------")]
    [SerializeField]
    protected Animator animator;


    [Header("-----------------Fase 1-----------------")]
    [Tooltip("Speed when the character is relax and in patrol state")]
    [SerializeField]
    protected float walkingSpeed = 0.1f;
    protected float radioDetection = 10f;

    [SerializeField]
    [Tooltip("Speed when the character is stressed or running away")]
    protected float run = 0.2f;

    [SerializeField]
    protected float rotateSpeed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    QuotaChecker quotaChecker;
    AnimalType animalType;


    void Start()
    {
        quotaChecker = GameFlowManager.instance.quotaChecker;

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
        return this.radioDetection;
    }
    public Animator GetAnimator()
    {
        return this.animator;
    }

    public Transform GetClosestObjetive()
    {
        throw new System.NotImplementedException();
    }



    private void OnDestroy()
    {
        quotaChecker.UpdateCuote(this.animalType, -1);
    }

    public void WalkRamdom(int limitSupX, int limitInfX, int limitSupY, int limitInfY)
    {
        //Vector3 newPos = new Vector3(Random.Range(limitInfX, limitSupX), Random.Range(limitInfY, limitSupY), transform.position.z);
        //Vector3 direccion = newPos - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(direccion);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
    public void WalkTo(Transform objetive)
    {
        Vector3 direccion = objetive.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        this.transform.LookAt(objetive);

        this.transform.position += direccion * this.walkingSpeed * Time.deltaTime;
    }

}
