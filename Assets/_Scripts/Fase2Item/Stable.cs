using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Stable : MonoBehaviour
{

    Dictionary<ItemNames, int> animalesEstablo = new Dictionary<ItemNames, int>();

    [SerializeField] RecipientController comedero;
    
    public static List<Stable> allStables = new List<Stable>();
    private void OnEnable()
    {
        allStables.Add(this);
    }
    private void OnDisable()
    {
        allStables.Remove(this);
    }

    public List<AAnimalFase2> animalesReferecia = new List<AAnimalFase2>();

    /// <summary>
    /// Returns all the animals with the specified ItemName
    /// </summary>
    /// <param name="itemNames"></param>
    /// <returns></returns>
    public int GetAnimalsInEstable(ItemNames itemNames)
    {
        if (animalesEstablo.ContainsKey(itemNames))
        {
            return animalesEstablo[itemNames];
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Returns all the animals in the stable
    /// </summary>
    /// <returns></returns>
    public int GetAnimalsInEstable()
    {
        int numAnimales = 0;
        foreach (ItemNames animName in animalesEstablo.Keys) {
            numAnimales += animalesEstablo[animName];
        }

        return numAnimales;
    }
    

    public Transform GetComedero()
    {
        return comedero.gameObject.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        AAnimalFase2 animal = other.gameObject.GetComponent<AAnimalFase2>();
        if(animal != null)
        {
            ItemNames nameAnim = animal.thisItemName;
            if (!animalesEstablo.ContainsKey(nameAnim))
            {
                animalesEstablo.Add(nameAnim, 1);
            }
            else
            {
                animalesEstablo[nameAnim]++;
            }
            animal.SetEstablo(this);
        }
    }

    public bool HayComida(ItemNames[] comidasPosibles)
    {
        return comedero.HayComida(comidasPosibles);
    }

    void OnTriggerExit(Collider other)
    {
        AAnimalFase2 animal = other.gameObject.GetComponent<AAnimalFase2>();
        if (animal != null)
        {
            ItemNames nameAnim = animal.thisItemName;
            animalesEstablo.TryGetValue(nameAnim, out int value);
            if (value != 0)
            {
                animalesEstablo[nameAnim]--;
            }
            else
            {
                Debug.Log("Hay algo raro se ha salido un animal que no debería estar aquí.");
            }
        }
    }

    public void ExitFromStable(ItemNames nameAnim)
    {
        animalesEstablo.TryGetValue(nameAnim, out int value);
        if (value != 0)
        {
            animalesEstablo[nameAnim]--;
        }
        else
        {
            Debug.Log("Hay algo raro se ha salido un animal que no debería estar aquí.");
        }

    }
}
