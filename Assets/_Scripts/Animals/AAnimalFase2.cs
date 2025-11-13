using BehaviourAPI.Core;
using System;
using UnityEngine;

public class AAnimalFase2: AAnimal
{
    [SerializeField] float MaxSinLimpiar;
    [SerializeField] float MaxSinComer;



    float tiempoSinLimpiar = 0f;
    float tiempoSinComer = 0f;

    #region Monobehavior

    protected override void Update()
    {
        base.Update();

        tiempoSinLimpiar += Time.deltaTime;
        tiempoSinComer += Time.deltaTime;
    }

    #endregion

    #region Actions
    public override void InitComer()
    {
        if (TieneComida())
        {
            lastObjectve = establo.GetComedero();
            RecipientController temp = lastObjectve.GetComponentInChildren<RecipientController>();
            if (temp) //se lo va a comer lit
            {
                if (animator)
                {
                    animator.SetTrigger("Comer");

                }
            }
        }
        //else if (!ObjectiveClose())
        //{
        //    lastObjectve = GetClosestObjetive();
        //    var temp = lastObjectve.GetComponentInChildren<ItemInScene>();
        //    if (temp) //se lo va a comer lit
        //    {
        //        if (animator)
        //        {
        //            animator.SetTrigger("Comer");
        //        }
        //    }
        //
        //}
        tiempoComiendo = 0.0f;

        
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
            var temp = lastObjectve.GetComponentInChildren<RecipientController>();
            if (temp) //se lo va a comer lit
            {
                temp.RemoveStack(objectives);
                tiempoSinComer = 0f;
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

    public override bool ObjectiveClose()
    {
        foreach(ItemNames objetivo in objectives)
        {
            if (establo.GetAnimalsInEstable(objetivo)>0)
            {
                return true;
            }
        }
        return false;
    }

    public override Transform GetClosestObjetive()
    {
        return IslandPositions.instance.GetClosest(establo.transform.position, objectives);
        
    }

    public void Enfermar()
    {
        throw new NotImplementedException();
    }

    public void Rascarse()
    {
        throw new NotImplementedException();
    }

    public void MostrarHambre()
    {
        throw new NotImplementedException();
    }

    public void MandarCorazones()
    {
        throw new NotImplementedException();
    }

    public void MostrarIncomodidad()
    {
        throw new NotImplementedException();
    }

    public void QuitarPegatinaEstado()
    {
        throw new NotImplementedException();
    }

    public void Limpiarse()
    {
        this.tiempoSinLimpiar = 0f;
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
        return tiempoSinLimpiar/MaxSinLimpiar;
    }
    public float TimeWithoutEating()
    {
        return tiempoSinComer/MaxSinComer;
    }

    public bool TieneComida()
    {
        return establo.HayComida(objectives);
    }

    public override Vector3 GetNewPosition()
    {
        Debug.LogError("Esto no debería estar siendo usado...");
        return Vector3.zero;
    }
    #endregion
}
