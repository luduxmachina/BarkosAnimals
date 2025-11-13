using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipientController : MonoBehaviour
{
    [SerializeField] int maxStacksFood = 3;
    int comidaStacks = 0;

    [SerializeField] List<ItemNames> tiposDeComidaAceptados = new List<ItemNames>();
    ItemNames tipoActual;

    public bool AddStack(ItemNames tipoComida)
    {
        if (comidaStacks >= maxStacksFood)
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
            return true;
        }
    }
}
