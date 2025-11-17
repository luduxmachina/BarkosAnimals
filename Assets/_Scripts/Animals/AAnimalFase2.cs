using BehaviourAPI.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AAnimalFase2: AAnimal
{
    [SerializeField] ItemNames itemName;
    public ItemNames thisItemName
    {
        get { return itemName; }
    }

    [Header("-----------------Fase 2-----------------")]
    [SerializeField] float MaxSinLimpiar;
    [SerializeField] float MaxSinComer;
    public float AMax = 0.0f;
    public float CMax = 0.0f;
    public float DMax = 0.0f;

    [SerializeField] protected Stable establo;
    [SerializeField] StikersManager stikersManager;
    [SerializeField] AllObjectTypesSO animalsDataBase;
    [SerializeField] NavMeshAgent navMeshAgent;

    public bool hayComida = false;

    [SerializeField, ReadOnly]bool isHerbivore = false;

    float tiempoSinLimpiar = 0f;
    float tiempoSinComer = 0f;

    public float depredadoresCerca = 0f;

    #region Monobehavior
    protected override void Awake()
    {

        ItemInScene thisItem = gameObject.GetComponentInChildren<ItemInScene>();
        itemName = thisItem.itemName;
        List<Restriction> restrict = animalsDataBase.GetRestrictions(itemName);
        if (restrict.Contains(Restriction.Herbivore))
        {
            isHerbivore = true;
        }
        else
        {
            isHerbivore = false;
        }
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if(tiempoSinLimpiar <= MaxSinLimpiar)tiempoSinLimpiar += Time.deltaTime;
        if(tiempoSinComer <= MaxSinComer)tiempoSinComer += Time.deltaTime;
        
        if(establo != null)
        {
            Debug.Log("Establo en el animal");

            //SistemaUtilidad.enabled = true;
        }
    }

    #endregion

    #region Setter
    public void SetEstablo(Stable establo)
    {
        this.establo = establo;
    }

    #endregion

    #region Actions
    public override void InitComer()
    {
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return;
        }
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
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return Status.Failure;
        }

        if (!TieneComida()) //la comida puede desaparecer
        {
            if (animator)
            {
                animator.SetTrigger("Idle");
        
            }
            tiempoComiendo = 0.0f;
        
            return Status.Failure;
        }
        if (Vector3.Distance(transform.position, lastObjectve.position) > radioAtaqueComida * 1.75) //alguien ha movido la comida o al animal y ya no esta comiendo lol
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

    public override Status MoveTowardsObjective()
    {
        if(establo == null)
        {
            return Status.Failure;
        }
        if (!TieneComida()) //la comida puede desaparecer
        {
            movimiento.CancelMove();

            return Status.Failure;
        }
        Transform objTR = establo.GetComedero().transform;
        Vector3 currentObjPos = objTR.position;

        if (lastTargetPos != currentObjPos)
        {
            lastTargetPos = currentObjPos;
            lastObjectve = objTR;
            //o se ha movido o un pan mas cercano
            movimiento.SetTarget(currentObjPos);

        }
        if (movimiento.HasArrived())
        {
            movimiento.CancelMove();

            return Status.Success;
        }


        return Status.Running;
    }

    public override bool ObjectiveClose()
    {
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return false;
        }
        foreach (ItemNames objetivo in objectives)
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
        stikersManager.SetImage(StikersGenerales.Enfermo);
    }

    public void Rascarse()
    {
        stikersManager.SetImage(StikersGenerales.NecesitaLimpiar);
        tiempoSinLimpiar -= (MaxSinLimpiar / 6);
    }

    public void MostrarHambre()
    {
        if (isHerbivore)
        {
            stikersManager.SetImage(StikersGenerales.NecesitaComerZanahoria);
        }
        else
        {
            stikersManager.SetImage(StikersGenerales.NecesitaComerCarne);
        }
    }

    public void MandarCorazones()
    {
        stikersManager.SetImage(StikersGenerales.Corazones);
    }

    public void MostrarIncomodidad()
    {
        stikersManager.SetImage(StikersGenerales.Incomodo);
    }

    public void QuitarPegatinaEstado()
    {
        stikersManager.OcultSprites();
    }

    public void Limpiarse()
    {
        this.tiempoSinLimpiar = 0f;
    }

    public override void Die()
    {
        GameFlowManager.instance.quotaChecker.UpdateCuote(new InventoryItemDataObjects(thisItemName, -1));

        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return;
        }
        establo.ExitFromStable(thisItemName);
        base.Die();
    }

    #endregion

    #region Pulls
    public float AnimalsOnEstable()
    {
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return 0f;
        }
        return ((float)establo.GetAnimalsInEstable()-1)/AMax;
    }

    public float ComaradesOnEstable()
    {
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return 0f;
        }

        float numCompis = 0;
        numCompis += establo.GetAnimalsInEstable(itemName);
        return (numCompis - 1)/CMax;//No le queremos contar a él mismo.
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
        depredadoresCerca = numPredators;

        return numPredators/DMax;
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

    public float HayComida()
    {
        hayComida = TieneComida();
        if (TieneComida())
        {
            return 1f;
        }
        return 0f;
    }

    public override Vector3 GetNewPosition()
    {
        Debug.LogError("Esto no debería estar siendo usado...");
        return Vector3.zero;
    }
    #endregion

    #region Fatores del sist de utilidad
    public float CurveFactorF4(float x)
    {
        return (Mathf.Log10(x + 1)/Mathf.Log10(2))
            /(Mathf.Log10(DMax - 1)/Mathf.Log10(2));
    }
    #endregion

}
