using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class ReproductionSpot : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] 
    private float babyScale = 0.5f;
    [SerializeField]
    private AnimalPlaceableSO animalPlaceableDB;

    private List<AnimalF2Instance> animalsInArea;
    private HashSet<int> babies;

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

            if (babies.Contains(hashCode))
                return;

            animalsInArea.Add(new AnimalF2Instance(hashCode, animalType));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<AAnimalFase2>(out var animal))
        {
            var animalType = animal.thisItemName;
            int hashCode = other.gameObject.GetHashCode();

            if (babies.Contains(hashCode))
                return;

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
            throw new FileNotFoundException();
        }

        baby.transform.localScale = Vector3.one * babyScale;
        Instantiate(baby, transform.position, Quaternion.identity);
        babies.Add(baby.GetHashCode());
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
