using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipientController : MonoBehaviour
{
    [SerializeField] int maxStacksFood = 3;
    int comidaStacks = 0;

    [SerializeField] GameObject comederoLleno;
    [SerializeField] GameObject comederoVacio;

    [SerializeField] List<ItemNames> tiposDeComidaAceptados = new List<ItemNames>();
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
                comederoLleno.SetActive(true);
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

    public bool Interact(ItemNames interactorType, GameObject interactor)
    {
        throw new System.NotImplementedException();
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
                comederoLleno.SetActive(false);
                comederoVacio.SetActive(true);
            }
            return true;
        }
    }
}
