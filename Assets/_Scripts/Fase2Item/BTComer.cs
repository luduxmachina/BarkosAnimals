using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UtilitySystems;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.StateMachines;

using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public class BTComer : BehaviourRunner
{
    [SerializeField] private bool useDebugger = false;
    [SerializeField, HideIf("useDebugger", false)] private BSRuntimeDebugger debuggerComponent;
    [SerializeField] private AAnimalFase2 m_AAnimalFase2;
    [SerializeField] private Transform Moverse_un_poco_action_OtherTransform;

    protected override void Init()
    {
        m_AAnimalFase2 = GetComponent<AAnimalFase2>();
        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
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
        Moverse_un_poco_action.OtherTransform = Moverse_un_poco_action_OtherTransform;
        Moverse_un_poco_action.speed = 3f;
        Moverse_un_poco_action.distance = 6f;
        Moverse_un_poco_action.maxTimeRunning = 20f;
        LeafNode Moverse_un_poco = Comer.CreateLeafNode("Moverse un poco", Moverse_un_poco_action);

        SequencerNode EATING = Comer.CreateComposite<SequencerNode>("EATING", false, Indica_que_tiene_Hambre, GoToEat, Eat, Moverse_un_poco);
        EATING.IsRandomized = false;

        LoopNode unnamed_3 = Comer.CreateDecorator<LoopNode>(EATING);
        unnamed_3.Iterations = -1;

        //el root es importante para los arboles
        Comer.SetRootNode(unnamed_3);

        return Comer;
    }
}
