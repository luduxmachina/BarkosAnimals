using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class ReproductionSpot : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] 
    private float babyScale = 0.5f;
    [SerializeField]
    private AnimalPlaceableSO animalPlaceableDB;

    private List<AnimalF2Instance> animalsInArea;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<AAnimalFase2>(out var animal))
        {
            var animalType = animal.thisItemName;
            int hashCode = other.gameObject.GetHashCode();

            animalsInArea.Add(new AnimalF2Instance(hashCode, animalType));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<AAnimalFase2>(out var animal))
        {
            var animalType = animal.thisItemName;
            int hashCode = other.gameObject.GetHashCode();

            foreach (var animalInArea in animalsInArea)
            {
                if (animalInArea.hashCode == hashCode)
                {
                    animalsInArea.Remove(animalInArea);
                    break;
                }
            }
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Vector3 GetWorldPosition()
    {
       return transform.position;
    }

    public int GetNumberOfAnimalsOfType(ItemNames animalType)
    {
        int num = 0;

        foreach (var item in animalsInArea)
        {
            if (item.animalType == animalType)
                ++num;
        }

        return num;
    }

    public void SpawnBabyOfType(ItemNames animalType)
    {

    }
}

class AnimalF2Instance
{
    public int hashCode { get; private set; }
    public ItemNames animalType { get; private set; }
    public AnimalF2Instance(int hashCode, ItemNames animalType)
    {
        this.hashCode = hashCode;
        this.animalType = animalType;
    }
}
