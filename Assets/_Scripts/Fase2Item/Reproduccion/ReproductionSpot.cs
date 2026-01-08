using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ReproductionSpot : MonoBehaviour
{
    public UnityEvent<ItemNames> OnAnimalEnterReproductionSpot = new UnityEvent<ItemNames>();

    private List<AnimalF2Instance> animalsInArea;
    private HashSet<GameObject> babies;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<AAnimalFase2>(out var animal))
        {
            var animalType = animal.thisItemName;
            GameObject animalObj = other.gameObject;

            if (babies.Contains(animalObj))
                return;

            animalsInArea.Add(new AnimalF2Instance(animalObj, animalType));
            OnAnimalEnterReproductionSpot.Invoke(animalType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<AAnimalFase2>(out var animal))
        {
            var animalType = animal.thisItemName;
            GameObject animalObj = other.gameObject;

            if (babies.Contains(animalObj))
                return;

            foreach (var animalInArea in animalsInArea)
            {
                if (animalInArea.animalObject == animalObj)
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

    public AnimalF2Instance ExtractAnimalOfType(ItemNames animalType)
    {
        for (int i = 0; i < animalsInArea.Count; i++)
        {
            if (animalsInArea[i].animalType == animalType)
            {
                AnimalF2Instance animal = animalsInArea[i];
                animalsInArea.RemoveAt(i);
                return animal;
            }
        }
        
        throw new Exception($"No animal found on reproduction spot with type [{animalType}]");
    }

    public void AddBabyToBlacklist(GameObject baby)
    {
        babies.Add(baby);
    }
}

public class AnimalF2Instance
{
    public GameObject animalObject { get; private set; }
    public ItemNames animalType { get; private set; }
    public AnimalF2Instance(GameObject animalObject, ItemNames animalType)
    {
        this.animalObject = animalObject;
        this.animalType = animalType;
    }
}
