using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public struct ComidaYComedero
{
    public ItemNames tipoComida;
    public GameObject comedero;
}

public class RecipientController : MonoBehaviour
{
    [SerializeField] int maxStacksFood = 3;
    int comidaStacks = 0;

    [SerializeField] GameObject comederoVacio;

    [SerializeField] List<ItemNames> tiposDeComidaAceptados = new List<ItemNames>();
    [SerializeField] List<ComidaYComedero> comidaYComederoList = new List<ComidaYComedero>();

    [SerializeField, ReadOnly] ItemNames tipoActual;

    public bool AddStack(ItemNames tipoComida)
    {
        if (tiposDeComidaAceptados.Contains(tipoComida))
        {
            if (comidaStacks >= maxStacksFood)
            {
                return false;
            }
            else
            {
                comidaStacks++;
                foreach (ComidaYComedero comedero in comidaYComederoList)
                {
                    if(comedero.tipoComida==tipoComida)comedero.comedero.SetActive(true);
                }
                comederoVacio.SetActive(false);
                tipoActual = tipoComida;
                return true;
            }
        }
        return false;
    }

    public bool HayComida(ItemNames[] tiposComida)
    {
        if (!tiposComida.ToList().Contains(tipoActual) && comidaStacks>0)
        {
            return false;
        }
        return comidaStacks > 0;
    }


    public bool RemoveStack(ItemNames[] tiposComida)
    {
        if (!tiposComida.ToList().Contains(tipoActual) || comidaStacks <= 0)
        {
            return false;
        }
        else
        {
            comidaStacks--;
            if(comidaStacks <= 0)
            {
                foreach (ComidaYComedero comedero in comidaYComederoList)
                {
                    comedero.comedero.SetActive(false);
                }
                comederoVacio.SetActive(true);
            }
            return true;
        }
    }
}
