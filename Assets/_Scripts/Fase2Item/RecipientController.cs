using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.UnityToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public struct ComidaYComedero
{
    public ItemNames tipoComida;
    public GameObject comedero;
}

public class RecipientController : MonoBehaviour, IRecipientControler
{
    [SerializeField] int maxStacksFood = 3;
    [SerializeField, ReadOnly] int comidaStacks = 0;

    [SerializeField] GameObject comederoVacio;

    [SerializeField] List<ItemNames> tiposDeComidaAceptados = new List<ItemNames>();
    [SerializeField] List<ComidaYComedero> comidaYComederoList = new List<ComidaYComedero>();

    [SerializeField, ReadOnly] ItemNames tipoActual;

    [SerializeField] TextMeshProUGUI textoContenido;

    [SerializeField] private Transform Moverse_un_poco_action_OtherTransform;

    UnityEvent ahoraHayComida = new UnityEvent();
    UnityEvent ahoraNoHayComida = new UnityEvent();


    public void SubscribeStable(Stable stable)
    {
        ahoraHayComida.AddListener(stable.HayComida);
        ahoraNoHayComida.AddListener(stable.NoHayComida);
    }

    private void Start()
    {
        textoContenido.text = comidaStacks.ToString() + "/" + maxStacksFood.ToString();
    }

    public bool AddStack(ItemNames tipoComida)
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
                }
            }
            else
            {
                comidaStacks = 1;
                ahoraHayComida.Invoke();
                Debug.Log("Llama a ahora hay comida");
                foreach (ComidaYComedero comedero in comidaYComederoList)
                {
                    if (comedero.tipoComida == tipoComida) comedero.comedero.SetActive(true);
                    else comedero.comedero.SetActive(false);
                }
                comederoVacio.SetActive(false);
                tipoActual = tipoComida;

            }

            textoContenido.text = comidaStacks.ToString()+"/"+maxStacksFood.ToString();

            return true;
        }
        return false;
    }

    public Transform GetTransfToEat(AAnimalFase2 animal)
    {
        return this.transform;
    }

    public bool HayComida(ItemNames[] tiposComida)
    {
        if (!tiposComida.ToList().Contains(tipoActual) && comidaStacks>0)
        {
            return false;
        }
        return comidaStacks > 0;
    }


    public bool RemoveStack(ItemNames[] tiposComida, AAnimalFase2 animal)
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
                ahoraNoHayComida.Invoke();
                foreach (ComidaYComedero comedero in comidaYComederoList)
                {
                    comedero.comedero.SetActive(false);
                }
                comederoVacio.SetActive(true);
                tipoActual = ItemNames.None;
            }
            textoContenido.text = comidaStacks.ToString() + "/" + maxStacksFood.ToString();
            return true;
        }
    }
    public BehaviourGraph CreateGraph(AAnimalFase2 m_AAnimalFase2)
    {
        BehaviourTree Comer = new BehaviourTree();
        SimpleAction Indica_que_tiene_Hambre_action = new SimpleAction();
        Indica_que_tiene_Hambre_action.action = m_AAnimalFase2.MostrarHambre;
        LeafNode Indica_que_tiene_Hambre = Comer.CreateLeafNode("Indica que tiene Hambre", Indica_que_tiene_Hambre_action);

        FunctionalAction GoToEat_action = new FunctionalAction();
        GoToEat_action.onStarted = m_AAnimalFase2.MoveTowardsObjectiveInit;
        GoToEat_action.onUpdated = m_AAnimalFase2.MoveTowardsObjective;
        LeafNode GoToEat = Comer.CreateLeafNode("GoToEat", GoToEat_action);

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

        SequencerNode EATING = Comer.CreateComposite<SequencerNode>("EATING", false, Indica_que_tiene_Hambre, GoToEat, Eat, Moverse_un_poco);
        EATING.IsRandomized = false;

        LoopNode loopComer = Comer.CreateDecorator<LoopNode>(EATING);
        loopComer.Iterations = -1;

        //el root es importante para los arboles
        Comer.SetRootNode(loopComer);

        return Comer;
    }

    public bool ComederoLibre(AAnimalFase2 animal)
    {
        return true;
    }
}
