using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.BehaviourTrees;

using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public class PatoBehaviour : BehaviourRunner
{
	[SerializeField] private PatoFase1 m_PatoFase1;


    [SerializeField] private BSRuntimeDebugger debuggerComponent;
    protected override void Init()
	{
		m_PatoFase1 = GetComponent<PatoFase1>();
		
		base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		BehaviourTree PatoBT = new BehaviourTree();
		
		FunctionalAction Andar_hacia_comida_action = new FunctionalAction();
		Andar_hacia_comida_action.onUpdated = m_PatoFase1.MoveTowardsObjective;
		LeafNode Andar_hacia_comida = PatoBT.CreateLeafNode("Andar hacia comida", Andar_hacia_comida_action);
		
		FunctionalAction Comer_action = new FunctionalAction();
		Comer_action.onStarted = m_PatoFase1.InitComer;
		Comer_action.onUpdated = m_PatoFase1.UpdateComer;
		LeafNode Comer = PatoBT.CreateLeafNode("Comer", Comer_action);
		
		SequencerNode unnamed_1 = PatoBT.CreateComposite<SequencerNode>(false, Andar_hacia_comida, Comer);
		unnamed_1.IsRandomized = false;
		
		ConditionNode Comer_si_hay_comida = PatoBT.CreateDecorator<ConditionNode>("Comer si hay comida", unnamed_1);
        ConditionPerception comidaCerca = new ConditionPerception();
        comidaCerca.onCheck = m_PatoFase1.ObjectiveClose;
        Comer_si_hay_comida.Perception = comidaCerca;


        FunctionalAction Volve_al_esanque_action = new FunctionalAction();
		Volve_al_esanque_action.onStarted = m_PatoFase1.IrAEstanqueInit;
		Volve_al_esanque_action.onUpdated = m_PatoFase1.IrAEstanqueUpdate;
		LeafNode Volve_al_esanque = PatoBT.CreateLeafNode("Volve al esanque", Volve_al_esanque_action);
		
		ConditionNode Volver_al_estanque_si_fuera_ = PatoBT.CreateDecorator<ConditionNode>("Volver al estanque si fuera ", Volve_al_esanque);
        ConditionPerception fueraEstanque = new ConditionPerception();
        fueraEstanque.onCheck = m_PatoFase1.FueraAgua;
        Volver_al_estanque_si_fuera_.Perception = fueraEstanque;


        PatrolAction Nada_action = new PatrolAction();
		Nada_action.maxDistance = 4.04f;
		LeafNode Nada = PatoBT.CreateLeafNode("Nada", Nada_action);
		
		SimpleAction CansarUnPoco_action = new SimpleAction();
		CansarUnPoco_action.action = m_PatoFase1.UsarEnergia;
		LeafNode CansarUnPoco = PatoBT.CreateLeafNode("CansarUnPoco", CansarUnPoco_action);
		
		SimpleAction Descansar_action = new SimpleAction();
		Descansar_action.action = m_PatoFase1.Descansar;
		LeafNode Descansar = PatoBT.CreateLeafNode("Descansar", Descansar_action);
		
		ConditionNode Ests_cansado = PatoBT.CreateDecorator<ConditionNode>("Ests cansado", Descansar);
        ConditionPerception estaCansado = new ConditionPerception();
        estaCansado.onCheck = m_PatoFase1.EstaCansado;
        Ests_cansado.Perception = estaCansado;

        SuccederNode unnamed_3 = PatoBT.CreateDecorator<SuccederNode>(Ests_cansado);
		
		SimpleAction aletarAction = new SimpleAction();
        aletarAction.action = m_PatoFase1.Aletear;
        DelayAction Aletear_action_delayed = new DelayAction();
        Aletear_action_delayed.delayTime = 2f;

		ParallelAction parallelAction = new ParallelAction(true, true, aletarAction, Aletear_action_delayed);
        LeafNode Aletear = PatoBT.CreateLeafNode("Aletear", parallelAction);
		
		SequencerNode unnamed_2 = PatoBT.CreateComposite<SequencerNode>(false, Nada, CansarUnPoco, unnamed_3, Aletear);
		unnamed_2.IsRandomized = false;
		
		ConditionNode Estar_en_agua = PatoBT.CreateDecorator<ConditionNode>("Estar en agua", unnamed_2);
        ConditionPerception estaEnAgua = new ConditionPerception();
        estaEnAgua.onCheck = m_PatoFase1.EnAgua;
        Estar_en_agua.Perception = estaEnAgua;


        SelectorNode unnamed = PatoBT.CreateComposite<SelectorNode>(false, Comer_si_hay_comida, Volver_al_estanque_si_fuera_, Estar_en_agua);
		unnamed.IsRandomized = false;
		
		LoopNode Root = PatoBT.CreateDecorator<LoopNode>("Root", unnamed);
		Root.Iterations = -1;
		
		PatoBT.SetRootNode(Root);

        debuggerComponent.RegisterGraph(PatoBT);
        return PatoBT;
	}
}
