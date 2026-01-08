using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using BehaviourAPI.Core;

public class Stable : MonoBehaviour
{

    Dictionary<ItemNames, int> animalesEstablo = new Dictionary<ItemNames, int>();
    public List<AAnimalFase2> animalesReferecia = new List<AAnimalFase2>();

    //[SerializeField] RecipientController comedero;

    [SerializeField]IRecipientControler comedero;
    
    public static List<Stable> allStables = new List<Stable>();

    private void OnEnable()
    {
        allStables.Add(this);
        if(comedero == null)
        {
            comedero = transform.parent.GetComponent<IRecipientControler>();
        }
        if(comedero != null)
        {
            comedero.SubscribeStable(this);
        }
    }

    private void OnDisable()
    {
        allStables.Remove(this);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;

        Queue<GameObject> allAnimalGO = new();
        foreach (var animal in animalesReferecia)
        {
            GameObject animalGO = animal.transform.parent.gameObject;
            if (animalGO != null)
                allAnimalGO.Enqueue(animalGO);
        }

        while (allAnimalGO.Count > 0)
        {
            Destroy(allAnimalGO.Dequeue());
        }
    }

    #region Comedero
    public void HayComida()
    {
        Console.WriteLine("Llama a hay comida");
        foreach (AAnimalFase2 animal in animalesReferecia)
        {
            animal.ChangeEatingAction(comedero.CreateGraph(animal));
        }
    }
    public void NoHayComida()
    {
        foreach (AAnimalFase2 animal in animalesReferecia)
        {
            animal.ChangeEatingAction(null);
        }
        //Debug.LogWarning("No debería entrar aquí");
    }

    public Transform GetComedero(AAnimalFase2 animal)
    {
        return comedero.GetTransfToEat(animal);
    }

    public bool HayComida(ItemNames[] comidasPosibles)
    {
        return comedero.HayComida(comidasPosibles);
    }

    public bool ComederoLibre(AAnimalFase2 animalFase2) { 
        return comedero.ComederoLibre(animalFase2);
    }

    #endregion

    #region Nido

    public Vector3 GetNidoPosition()
    {
        return transform.parent.GetComponentInChildren<ReproductionSpot>().GetWorldPosition();

    }

    #endregion

    #region Animales
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
        numAnimales = animalesReferecia.Count;

        return numAnimales;
    }
    public int GetAnimalsInEstable(ItemNames[] tipos)
    {
        List<ItemNames> tiposAnim = tipos.ToList();
        int numAnimales = 0;
        foreach (ItemNames animName in animalesEstablo.Keys)
        {
            if(tiposAnim.Contains(animName)) numAnimales += animalesEstablo[animName];
        }

        return numAnimales;
    }

    public int GetHappyAnimals()
    {
        int numAnimales = 0;
        foreach (AAnimalFase2 animal in animalesReferecia)
        {
            if (animal.IsHappy)
            {
                numAnimales++;
            }
        }

        return numAnimales;
    }

    public AAnimalFase2 GetAnimalFromTypes(ItemNames[] tipos)
    {
        List<ItemNames>tiposAnim = tipos.ToList();

        foreach (AAnimalFase2 animal in animalesReferecia)
        {
            if (tiposAnim.Contains(animal.thisItemName)){
                return animal;
            }
        }
        return null;
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
            animalesReferecia.Add(animal);
        }
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
                Debug.Log("Hay algo raro se ha salido un animal que no deber�a estar aqu�.");
            }
            animalesReferecia.Remove(animal);
        }
    }

    public void ExitFromStable(AAnimalFase2 animal)
    {
        animalesEstablo.TryGetValue(animal.thisItemName, out int value);
        if (value != 0)
        {
            animalesEstablo[animal.thisItemName]--;
            animalesReferecia.Remove(animal);
        }
        else
        {
            Debug.Log("Hay algo raro se ha salido un animal que no deber�a estar aqu�.");
        }

    }
    #endregion
}
