using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.SmartObjects;
using BehaviourAPI.UnityToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Comedero
{
    [SerializeField] public Transform pos;
    [SerializeField] public GameObject comederoVacio;
    [SerializeField] public List<ComidaYComedero> comidaYComederoList = new();
    [SerializeField] public bool ocupado = false;
    [SerializeField]public bool hasFood = false;
    [SerializeField] public AAnimalFase2 animalOcupando;
}

public class MultipleRecipientController : IRecipientControler
{
    [SerializeField] int maxStacksFood = 3;
    [SerializeField, ReadOnly] int comidaStacks = 0;
    int comidaSinComedero = 0;

    [SerializeField] List<ItemNames> tiposDeComidaAceptados = new List<ItemNames>();

    [SerializeField] List<Comedero> comederos = new List<Comedero>();

    [SerializeField, ReadOnly] ItemNames tipoActual;

    [SerializeField] TextMeshProUGUI textoContenido;

    [SerializeField] private Transform Moverse_un_poco_action_OtherTransform;

    UnityEvent ahoraHayComida = new UnityEvent();
    UnityEvent ahoraNoHayComida = new UnityEvent();


    public override void SubscribeStable(Stable stable)
    {
        ahoraHayComida.AddListener(stable.HayComida);
        ahoraNoHayComida.AddListener(stable.NoHayComida);
    }

    private void Start()
    {
        textoContenido.text = comidaStacks.ToString() + "/" + maxStacksFood.ToString();
    }

    public override bool AddStack(ItemNames tipoComida)
    {
        if (tiposDeComidaAceptados.Contains(tipoComida))
        {
            if(tipoComida == tipoActual)
            {
                if (comidaStacks >= maxStacksFood)
                {
                    return false;
                }
                else
                {
                    comidaStacks++;

                    bool comidaAñadida = false;
                    foreach (Comedero c in comederos)
                    {
                        if (c.hasFood) continue;
                        c.hasFood = true;
                        comidaAñadida = true;
                        foreach (ComidaYComedero comedero in c.comidaYComederoList)
                        {
                            if (comedero.tipoComida == tipoComida) comedero.comedero.SetActive(true);
                            else comedero.comedero.SetActive(false);
                        }

                        c.comederoVacio.SetActive(false);
                        break;
                    }
                    if (!comidaAñadida) comidaSinComedero++;
                }
            }
            else
            {
                comidaStacks = 1;
                ahoraHayComida.Invoke();
                Debug.Log("Llama a ahora hay comida");

                bool primero = true;

                foreach(Comedero c in comederos)
                {
                    if(primero)
                    {
                        primero = false;
                        c.hasFood = true;

                        foreach (ComidaYComedero comedero in c.comidaYComederoList)
                        {
                            if (comedero.tipoComida == tipoComida) comedero.comedero.SetActive(true);
                            else comedero.comedero.SetActive(false);
                        }
                        c.comederoVacio.SetActive(false);
                    }
                    else
                    {
                        foreach (ComidaYComedero comedero in c.comidaYComederoList)
                        {
                            comedero.comedero.SetActive(false);
                        }
                        c.comederoVacio.SetActive(true);
                    }
                }
                tipoActual = tipoComida;

            }

            textoContenido.text = comidaStacks.ToString()+"/"+maxStacksFood.ToString();

            return true;
        }
        return false;
    }

    public override Transform GetTransfToEat(AAnimalFase2 animal)
    {
        foreach(Comedero comedero in comederos)
        {
            if(comedero.animalOcupando == animal && comedero.ocupado)
            {
                Debug.Log("Animal va al comedero específico: " +comedero.pos.position);
                return comedero.pos;
            }
        }
        Debug.Log("Animal va a comedero en general "+transform.position);
        return this.transform;
    }

    public override bool HayComida(ItemNames[] tiposComida)
    {
        if (!tiposComida.ToList().Contains(tipoActual) && comidaStacks>0)
        {
            return false;
        }
        return comidaStacks > 0;
    }

    public override bool ComederoLibre(AAnimalFase2 animal)
    {
        foreach(Comedero c in comederos)
        {
            if(!c.ocupado && c.hasFood || c.animalOcupando == animal && c.ocupado)
            {
                Debug.Log("Hay un comedero libre");
                c.ocupado = true;
                c.animalOcupando = animal;
                return true;
            }
        }
        return false;
    }


    public override bool RemoveStack(ItemNames[] tiposComida, AAnimalFase2 animal)
    {
        if (!tiposComida.ToList().Contains(tipoActual) || comidaStacks <= 0)
        {
            return false;
        }
        else
        {
            comidaStacks--;
            Debug.Log("Esta es al comida sin colocar: " + comidaSinComedero);
            foreach (Comedero c in comederos)
            {
                if (comidaSinComedero <= 0)
                {
                    if (c.animalOcupando == animal && c.ocupado && c.hasFood)
                    {
                        c.ocupado = false;
                        c.hasFood = false;
                        foreach (ComidaYComedero comedero in c.comidaYComederoList)
                        {
                            comedero.comedero.SetActive(false);
                        }
                        c.comederoVacio.SetActive(true);
                    }
                }
                else
                {
                    c.ocupado = false;
                    comidaSinComedero--;
                }
            }

            if (comidaStacks <= comidaSinComedero)
            {
                Debug.LogWarning("No se está reduciendo correctamente la comida");
            }

            if(comidaStacks <= 0)
            {
                ahoraNoHayComida.Invoke();
                tipoActual = ItemNames.None;
            }
            textoContenido.text = comidaStacks.ToString() + "/" + maxStacksFood.ToString();
            return true;
        }
    }

    public override SmartInteraction RequestInteraction(SmartAgent agent, RequestData data)
    {
        AAnimalFase2 m_AAnimalFase2 = agent.GetComponent<AAnimalFase2>();

        Dictionary<string, float> capabilityMap = new();

        BehaviourTree Comer = new BehaviourTree();

        SimpleAction Indica_que_tiene_Hambre_action = new SimpleAction();
        Indica_que_tiene_Hambre_action.action = m_AAnimalFase2.MostrarHambre;
        LeafNode Indica_que_tiene_Hambre = Comer.CreateLeafNode("Indica que tiene Hambre", Indica_que_tiene_Hambre_action);

        FunctionalAction GoToEat_action = new FunctionalAction();
        GoToEat_action.onStarted = m_AAnimalFase2.MoveTowardsObjectiveInit;
        GoToEat_action.onUpdated = m_AAnimalFase2.MoveTowardsObjective;
        LeafNode GoToEat = Comer.CreateLeafNode("GoToEat", GoToEat_action);
        LeafNode GoToEat2 = Comer.CreateLeafNode("GoToEat2", GoToEat_action);
        LeafNode GoToEat3 = Comer.CreateLeafNode("GoToEat3", GoToEat_action);

        FunctionalAction Eat_action = new FunctionalAction();
        Eat_action.onStarted = m_AAnimalFase2.InitComer;
        Eat_action.onUpdated = m_AAnimalFase2.UpdateComer;
        LeafNode Eat = Comer.CreateLeafNode("Eat", Eat_action);
        

        FleeAction Moverse_un_poco_action = new FleeAction();
        Moverse_un_poco_action.OtherTransform =  Moverse_un_poco_action_OtherTransform;
        Moverse_un_poco_action.speed = 3f;
        Moverse_un_poco_action.distance = 6f;
        Moverse_un_poco_action.maxTimeRunning = 20f;
        LeafNode Moverse_un_poco = Comer.CreateLeafNode("Moverse un poco", Moverse_un_poco_action);
        LeafNode Moverse_un_poco2 = Comer.CreateLeafNode("Moverse un poco2", Moverse_un_poco_action);

        SequencerNode COMEDERO = Comer.CreateComposite<SequencerNode>("COMEDERO1", false, GoToEat, Eat);

        ConditionNode Comedero_Tiene_Comida = Comer.CreateDecorator<ConditionNode>(COMEDERO);
        Comedero_Tiene_Comida.Perception = new ConditionPerception(m_AAnimalFase2.ComprobarComedero);

        SequencerNode IRSEUNRATO = Comer.CreateComposite<SequencerNode>("IrseUnRato", false, Moverse_un_poco, GoToEat2);

        SuccederNode succederNode = Comer.CreateDecorator<SuccederNode>(IRSEUNRATO);

        InverterNode inverterNode = Comer.CreateDecorator<InverterNode>(succederNode);

        SelectorNode COMER = Comer.CreateComposite<SelectorNode>("COMER", false, Comedero_Tiene_Comida, inverterNode); 

        LoopUntilNode Comer_En_Comedero = Comer.CreateDecorator<LoopUntilNode>(COMER);
        Comer_En_Comedero.TargetStatus = Status.Success;

        SequencerNode EATING = Comer.CreateComposite<SequencerNode>("EATING", false, Indica_que_tiene_Hambre, GoToEat3, Comer_En_Comedero, Moverse_un_poco2);
        EATING.IsRandomized = false;

        LoopNode loopComer = Comer.CreateDecorator<LoopNode>(EATING);
        loopComer.Iterations = -1;

        //el root es importante para los arboles
        Comer.SetRootNode(loopComer);

        SubsystemAction comer = new SubsystemAction(Comer);
        Debug.Log("Crea el grafo de comer.");

        return new SmartInteraction(comer,agent, capabilityMap);
    }
    public override bool ValidateAgent(SmartAgent agent)
    {
        return true;
    }
    public override float GetCapabilityValue(string needName)
    {
        return 0f;
    }

}
