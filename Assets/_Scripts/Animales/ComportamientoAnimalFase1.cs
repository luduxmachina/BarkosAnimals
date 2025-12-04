using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;


public class ComportamientoAnimalFase1 : BehaviourRunner
{
	[SerializeField] private bool useDebugger = false;
    [SerializeField, HideIf("useDebugger", false)] private BSRuntimeDebugger debuggerComponent;

    [SerializeField] private AAnimal Huir_action_animalContext;
	[SerializeField] private Transform Huir_action_OtherTransform;
	[SerializeField] private AAnimal m_AAnimal;

    private float runSpeed;
    private float runAwayDistance;
    protected override void Init()
	{
		m_AAnimal = GetComponent<AAnimal>();

        runSpeed = m_AAnimal.GetRunSpeed();
        runAwayDistance = m_AAnimal.GetRadioAwareness() * 1.25f;
        base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		FSM HuirNoHuir = new FSM();
		BehaviourTree newbehaviourgraph = new BehaviourTree();
		
		SubsystemAction Tranquilo_action = new SubsystemAction(newbehaviourgraph);
		State Tranquilo = HuirNoHuir.CreateState("Tranquilo", Tranquilo_action);
		
		FleeActionNoRecto Huir_action = new FleeActionNoRecto();
		Huir_action.animalContext = Huir_action_animalContext;
		Huir_action.angulo = 0f;
		Huir_action.OtherTransform = Huir_action_OtherTransform;
        Huir_action.speed = runSpeed;
        Huir_action.distance = runAwayDistance;
        Huir_action.maxTimeRunning = 20f;
		State Huir = HuirNoHuir.CreateState("Huir", Huir_action);

        ConditionPerception DepredadorTodaviaCerca_perception = new ConditionPerception();

        DepredadorTodaviaCerca_perception.onCheck = m_AAnimal.PredatorClose;
        StateTransition DepredadorTodaviaCerca = HuirNoHuir.CreateTransition("DepredadorTodaviaCercaCerca", Huir, Huir, DepredadorTodaviaCerca_perception);
		DepredadorTodaviaCerca.StatusFlags = StatusFlags.Finished;

        ConditionPerception DepredadorCerca_perception = new ConditionPerception();
		DepredadorCerca_perception.onCheck = m_AAnimal.PredatorClose;
		SimpleAction DepredadorCerca_action = new SimpleAction();
		DepredadorCerca_action.action = m_AAnimal.PlayRunAnim;
		StateTransition DepredadorCerca = HuirNoHuir.CreateTransition("DepredadorCerca", Tranquilo, Huir, DepredadorCerca_perception, DepredadorCerca_action);
		


		ConditionPerception DepredadoLejos_perception = new ConditionPerception();
		DepredadoLejos_perception.onCheck = m_AAnimal.NotPredatorClose;
		StateTransition DepredadoLejos = HuirNoHuir.CreateTransition("DepredadoLejos", Huir, Tranquilo, DepredadoLejos_perception);
		
		PatrolAction Patrullar_action = new PatrolAction();
		Patrullar_action.maxDistance = m_AAnimal.GetPatrolRadius();
		LeafNode Patrullar = newbehaviourgraph.CreateLeafNode("Patrullar", Patrullar_action);
		
		DelayAction TiempoDeEspera_action = new DelayAction();
		TiempoDeEspera_action.delayTime = m_AAnimal.GetTiempoEnComer();
		LeafNode TiempoDeEspera = newbehaviourgraph.CreateLeafNode("TiempoDeEspera", TiempoDeEspera_action);
		
		SimpleAction Paron_action = new SimpleAction();
		Paron_action.action = m_AAnimal.PlayIdleAnim;
		LeafNode Paron = newbehaviourgraph.CreateLeafNode("Paron", Paron_action);
		
		SequencerNode unnamed_1 = newbehaviourgraph.CreateComposite<SequencerNode>(false, Patrullar, TiempoDeEspera, Paron);
		unnamed_1.IsRandomized = false;
		
		LoopNode unnamed = newbehaviourgraph.CreateDecorator<LoopNode>(unnamed_1);
		unnamed.Iterations = -1;
		newbehaviourgraph.SetRootNode(unnamed);

		if (useDebugger)
		{

			debuggerComponent.RegisterGraph(HuirNoHuir);
			debuggerComponent.RegisterGraph(newbehaviourgraph);
		}
        return HuirNoHuir;
	}
}
