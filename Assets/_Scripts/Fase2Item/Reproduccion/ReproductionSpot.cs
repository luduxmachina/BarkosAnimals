using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ReproductionSpot : MonoBehaviour
{
    public UnityEvent<ItemNames> OnAnimalEnterReproductionSpot = new UnityEvent<ItemNames>();

    private List<AnimalF2Instance> animalsInArea = new();

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

            if (CheckIfIsBaby(animalObj))
            {
                // Informar al bebe de que se aleje
                var pushPerception = gameObject.GetComponentInChildren<AAnimalFase2>().GetPerceptioStopReproducing();
                pushPerception.Fire();
                return;
            }
            
            animalsInArea.Add(new AnimalF2Instance(animalObj, animalType));
            OnAnimalEnterReproductionSpot.Invoke(animalType);
        }
    }

    private bool CheckIfIsBaby(GameObject animalObj)
    {
        if(animalObj.transform.parent == GetComponent<ReproductionManager>().babiesTransform)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<AAnimalFase2>(out var animal))
        {
            var animalType = animal.thisItemName;
            GameObject animalObj = other.gameObject;

            if (CheckIfIsBaby(animalObj))
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
