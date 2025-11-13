using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stable : MonoBehaviour
{
    [SerializeField] int maxStacksFood = 3;

    Dictionary<ItemNames, int> animalesEstablo = new Dictionary<ItemNames, int>();

    public int comidaStacks = 0;

    [SerializeField] List<ItemNames> tiposDeComidaAceptados = new List<ItemNames>();
    ItemNames tipoActual;

    [SerializeField] Transform comedero;


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

    public bool AddStack(ItemNames tipoComida)
    {
        if(comidaStacks >= maxStacksFood)
        {
            return false;
        }
        else
        {
            comidaStacks++;
            return true;
        }
    }

    public bool HayComida(ItemNames[] tiposComida)
    {
        if (!tiposComida.ToList().Contains(tipoActual))
        {
            return false;
        }
        return comidaStacks > 0;
    }

    public bool RemoveStack(ItemNames[] tiposComida)
    {
        if (!tiposComida.ToList().Contains(tipoActual) || comidaStacks <= 0)
        {
            return false ;
        }
        else
        {
            comidaStacks--;
            return true;
        }
    }

    public Transform GetComedero()
    {
        return comedero;
    }

    void OnTriggerEnter(Collider other)
    {
        AAnimal animal = other.gameObject.GetComponent<AAnimal>();
        if(animal != null)
        {
            ItemNames nameAnim = animal.ThisItemName;
            if (!animalesEstablo.ContainsKey(nameAnim))
            {
                animalesEstablo.Add(nameAnim, 1);
            }
            else
            {
                animalesEstablo[nameAnim]++;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        AAnimal animal = other.gameObject.GetComponent<AAnimal>();
        if (animal != null)
        {
            ItemNames nameAnim = animal.ThisItemName;
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
}
