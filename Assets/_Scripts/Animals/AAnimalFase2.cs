using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] float suciedadMaxima;
    [SerializeField] float MaxSinComer;
    [SerializeField] float TiempoEnfermoHastaMorir = 60;
    public float AMax = 0.0f;
    public float CMax = 0.0f;
    public float DMax = 0.0f;

    [SerializeField] protected Stable establo;
    [SerializeField] StikersManager stikersManager;
    [SerializeField] StikersManager stickerLimpieza;
    //[SerializeField] AllObjectTypesSO animalsDataBase;
    public bool useEditorBehaviour = true;
    [SerializeField, HideIf("useEditorBehaviour", false)] EditorBehaviourRunner SistemaUtilidadEBR;
    [SerializeField, HideIf("useEditorBehaviour", true)] AnimalesF2US SistemaUtilidadCode;

    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Predicate<float> funcionFelicidad;
    [SerializeField] DirtCreator dirtCreator;


    public bool hayComida = false;

    [SerializeField, ReadOnly]bool isHerbivore = false;

    float suciedad = 0f;

    [Tooltip("Timpo que pasa hasta que comprueba que está sucio.")]
    public float tiempoHastaSucio = 5f;
    float tiempoSinLimpiar = 0f;

    [SerializeField, ReadOnly]
    public float Suciedad => suciedad;
    public float suciedadQueQuita = 10f;
    float tiempoSinComer = 0f;

    public float depredadoresCerca = 0f;
    bool estaEnfermo;
    float tiempoEnfermo;

    bool isHappy;
    public bool IsHappy => isHappy;

    #region Monobehavior
    protected override void Awake()
    {


        List<ItemNames> list = objectives.ToList();
        if (list.Contains(ItemNames.Bread))
        {
            isHerbivore = true;
        }
        else
        {
            isHerbivore = false;
        }
        base.Awake();
        dirtCreator = FindAnyObjectByType<DirtCreator>();
    }

    protected override void Start()
    {
        base.Start();
        //if (useEditorBehaviour) SistemaUtilidadEBR.enabled = false;
        //else SistemaUtilidadCode.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (suciedad <= suciedadMaxima && tiempoSinLimpiar >= tiempoHastaSucio)
        {            
            suciedad += dirtCreator.GetHowMuchDirtIsNear(this.transform.position, 3f);
            Debug.Log($"Suciedad cerca de {gameObject.name}: {dirtCreator.GetHowMuchDirtIsNear(this.transform.position, 3f)} y tiene suciedad de {suciedad}");
            tiempoSinLimpiar = 0f;
        }
        tiempoSinLimpiar += Time.deltaTime;

        if(tiempoSinComer <= MaxSinComer)tiempoSinComer += Time.deltaTime;
        if (estaEnfermo) { tiempoEnfermo += Time.deltaTime; }
        if(tiempoEnfermo > TiempoEnfermoHastaMorir)
        {
            this.Die();
            tiempoEnfermo = 0f;
        }
        
        bool SUEnabled = (useEditorBehaviour)?SistemaUtilidadEBR.enabled : SistemaUtilidadCode.enabled;

        if(establo != null && !SUEnabled)
        {
            Debug.Log("Establo en el animal");
            if(useEditorBehaviour)SistemaUtilidadEBR.enabled = true;
            else SistemaUtilidadCode.enabled = true;
            //navMeshAgent.enabled = true;
        }
    }

    private void OnDisable()
    {
        establo?.ExitFromStable(this);  
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
        lastObjectve = GetClosestObjetive();
        if (lastObjectve != null)
        {
            if (animator)
            {
                animator.SetTrigger("Comer");

            }
            tiempoComiendo = 0.0f;
            if (!TieneComidaEnEstablo())
            {
                this.GetComponentInChildren<SimpleGrabber>().TryGrab(lastObjectve);
            }
        }

    }

    public override bool ObjectiveClose()
    {
        return TieneComidaEnEstablo() || establo.GetAnimalsInEstable(objectives) > 0;
    }

    public override Transform GetClosestObjetive()
    {
        if (TieneComidaEnEstablo())
        {
            return establo.GetComedero();
        }
        if(establo.GetAnimalsInEstable(objectives) > 0)
        {
            AAnimalFase2 animal = establo.GetAnimalFromTypes(objectives);
            return animal.transform;
        }
        return null;
    }


    public override Status UpdateComer()
    {
        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return Status.Failure;
        }

        if (lastObjectve == null) 
        {
            if (animator)
            {
                animator.SetTrigger("Idle");
        
            }
            tiempoComiendo = 0.0f;
        
            return Status.Failure;
        }

        Transform newObjetive = GetClosestObjetive();
        if(!newObjetive == lastObjectve) return Status.Failure;

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
            else
            {
                var temp2 = lastObjectve.GetComponentInChildren<ItemInScene>();
                if (temp2) //se lo va a comer lit
                {
                    temp2.ReduceByOne();
                    tiempoSinComer = 0f;
                }
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
        return base.MoveTowardsObjective();
    }

    public void Enfermar()
    {
        stikersManager.SetImage(StikersGenerales.Enfermo);
        tiempoEnfermo = 0.0f;
        estaEnfermo = true;
        
    }
    public void YaNoEstaEnfermo()
    {
        estaEnfermo = false;
    }

    public void Rascarse()
    {
        stikersManager.SetImage(StikersGenerales.NecesitaLimpiar);
        suciedad -= (suciedadMaxima / 6);
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
        if(!isHappy)GameFlowManager.instance.quotaChecker.UpdateQuoteWithHappinesOfAnimal(true);
        isHappy = true;
    }

    public void YaNoEstaContento()
    {
        if(isHappy)GameFlowManager.instance.quotaChecker.UpdateQuoteWithHappinesOfAnimal(false);
        isHappy = false;
    }

    public void NoMostrarNada()
    {
        stikersManager.HideSprites();
    }

    public void MostrarIncomodidad()
    {
        stikersManager.SetImage(StikersGenerales.Incomodo);
    }

    public void Limpiar()
    {
        this.suciedad -= suciedadQueQuita;
        stickerLimpieza.SetImage(StikersGenerales.Cansancio);
        Debug.Log("Esta limpiandose");
    }

    public override void Die()
    {
        GameFlowManager.instance.quotaChecker.UpdateCuote(new InventoryItemDataObjects(thisItemName, -1));

        if (establo == null)
        {
            Debug.LogWarning("El pato no está en ningún establo");
            return;
        }
        establo.ExitFromStable(this);
        ItemInScene snake = GetComponentInChildren<ItemInScene>();
        snake.ReduceByOne();
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
        return suciedad/suciedadMaxima;
    }
    public float TimeWithoutEating()
    {
        return tiempoSinComer/MaxSinComer;
    }

    public bool TieneComidaEnEstablo()
    {
        return establo.HayComida(objectives);
    }

    public float PuedeComer()
    {
        if (HayComida()) return 1f;
        else return 0f;
    }

    public bool HayComida()
    {
        hayComida = TieneComidaEnEstablo();
        if (TieneComidaEnEstablo() || establo.GetAnimalsInEstable(objectives) > 0)
        {
            return true;
        }
        return false;
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
