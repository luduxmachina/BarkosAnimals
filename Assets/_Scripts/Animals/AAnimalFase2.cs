using BehaviourAPI.Core;
using System;
using UnityEngine;

public class AAnimalFase2: AAnimal
{
    #region Actions
    public override void InitComer()
    {
        if (!TieneComida()) { return; }
        lastObjectve = establo.GetComedero();
        var temp = lastObjectve.GetComponentInChildren<ItemInScene>();
        if (temp) //se lo va a comer lit
        {
            if (animator)
            {
                animator.SetTrigger("Comer");

            }
        }
        tiempoComiendo = 0.0f;

        //  if (!) { return;  }
        //animator supongo
        //comer el pan
        //  comidaObjetivo.GetComponentInChildren<ItemInScene>()?.ReduceByOne();
    }
    public override Status UpdateComer()
    {
        if (!TieneComida()) //la comida puede desaparecer
        {
            if (animator)
            {
                animator.SetTrigger("Idle");

            }
            tiempoComiendo = 0.0f;

            return Status.Failure;
        }
        if (Vector3.Distance(transform.position, lastObjectve.position) > radioAtaqueComida * 1.25) //alguien ha movido la comida o al animal y ya no esta comiendo lol
        {
            if (animator)
            {
                animator.SetTrigger("Idle");

            }
            tiempoComiendo = 0.0f;

            return Status.Failure;
        }

        tiempoComiendo += Time.deltaTime;
        if (tiempoComiendo >= tiempoEnComer)
        {
            var temp = lastObjectve.GetComponentInChildren<ItemInScene>();
            if (temp) //se lo va a comer lit
            {
                temp.ReduceByOne();
            }
            if (animator)
            {
                animator.SetTrigger("Idle");

            }
            tiempoComiendo = 0.0f;
            return Status.Success;
        }
        return Status.Running;
    }

    public void GetObjetivo()
    {

    }

    public void Enfermar()
    {
        throw new NotImplementedException();
    }

    public void MostrarHambre()
    {

    }

    public void QuitarPegatinaEstado()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Pulls
    public float AnimalsOnEstable()
    {
        return (float)establo.GetAnimalsInEstable();
    }

    public float ComaradesOnEstable()
    {
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return 0f;
        }

        float numPredators = 0;
        numPredators += establo.GetAnimalsInEstable(itemName);
        return numPredators - 1;//No le queremos contar a él mismo.
    }

    public float PredatorsOnEstable()
    {
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return 0f;
        }

        float numPredators = 0;
        foreach (ItemNames predator in this.predators)
        {
            numPredators += establo.GetAnimalsInEstable(predator);
        }
        return numPredators;
    }

    public float TimeWithoutShower()
    {
        throw new NotImplementedException();
    }
    public float TimeWithoutEating()
    {
        throw new NotImplementedException();
    }

    public bool TieneComida()
    {
        return establo.HayComida(objectives);
    }

    public override Vector3 GetNewPosition()
    {
        throw new NotImplementedException();
    }
    #endregion
}
