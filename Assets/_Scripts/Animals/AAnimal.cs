using UnityEngine;

public abstract class AAnimal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    QuotaChecker quotaChecker;
    AnimalType animalType;


    void Start()
    {
        quotaChecker = GameFlowManager.instance.quotaChecker;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        quotaChecker.UpdateCuote(this.animalType, -1);
    }

    protected abstract void WalkRamdom();
    protected abstract void WalkTo();
    protected abstract void Eat();
    protected abstract void ScapeFrom();
    protected abstract void RunTo();
    protected abstract void Attack();

}
