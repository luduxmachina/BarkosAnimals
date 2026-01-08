using System;
using UnityEngine;
[RequireComponent(typeof(ReproductionSpot))]

public class ReproductionManager : MonoBehaviour
{
    [SerializeField]
    private AnimalPlaceableSO animalPlaceableDB;
    [SerializeField, Range(0f, 1f)] 
    private float babyScale = 0.5f;
    [SerializeField, Range(0f, 1f)] 
    private float spawnHeight = 0.2f;
    [SerializeField, Range(1f, 5f)] 
    private int numOfAnimalsNeededToReproduce = 2;
    [SerializeField] public Transform babiesTransform;
    
    private ReproductionSpot reproductionSpot;
    
    private void Awake()
    {
        reproductionSpot = GetComponent<ReproductionSpot>();
        reproductionSpot.OnAnimalEnterReproductionSpot.AddListener((animalType) => TryToReproduceAnimalOfType(animalType));
    }

    private bool TryToReproduceAnimalOfType(ItemNames animalType)
    {
        return TryToSpawnBabyOfType(animalType);
    }
    
    private bool TryToSpawnBabyOfType(ItemNames animalType)
    {
        if (reproductionSpot.GetNumberOfAnimalsOfType(animalType) >= numOfAnimalsNeededToReproduce)
        {
            SpawnBabyOfType(animalType);
            InformAnimalsToStopReproduction(animalType);
            
            return true;
        }
        
        return false;
    }

    private void InformAnimalsToStopReproduction(ItemNames animalType)
    {
        for (int i = 0; i < numOfAnimalsNeededToReproduce; i++)
        {
            GameObject animal = reproductionSpot.ExtractAnimalOfType(animalType).animalObject;
            
            // Informar al animal de que ya puede dejar de intentar reproducirse
            var pushPerception = animal.GetComponent<AAnimalFase2>().GetPerceptioStopReproducing();
            pushPerception.Fire();
        }
    }
    
    private void SpawnBabyOfType(ItemNames animalType)
    {
        GameObject baby = null;
        foreach (var item in animalPlaceableDB.GetPlaceableObjects())
        {
            ItemNames currentAnimaltype = item.Prefab.gameObject.GetComponentInChildren<AAnimalFase2>().thisItemName;
            if(currentAnimaltype == animalType)
            {
                baby = item.Prefab;
                break;
            }
        }

        if(baby == null)
        {
            throw new Exception($"No animal found on database with type [{animalType}]");
        }
        
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);
        GameObject babyInstance = Instantiate(baby, pos, Quaternion.identity, babiesTransform);
        babyInstance.transform.localScale = babyInstance.transform.localScale * babyScale;

    }
}
