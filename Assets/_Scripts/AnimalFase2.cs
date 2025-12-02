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

public class AnimalFase2 : BehaviourRunner
{
	[SerializeField] private AAnimalFase2 m_AAnimalFase2;
	[SerializeField] private Transform Moverse_un_poco_action_OtherTransform;
	
	protected override void Init()
	{
		m_AAnimalFase2 = GetComponent<AAnimalFase2>();
		
		base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		UtilitySystem Fase2US = new UtilitySystem(1f);
		BehaviourTree Comer = new BehaviourTree();
		FSM EstaFeliz = new FSM();
		
		VariableFactor TC = Fase2US.CreateVariable(m_AAnimalFase2.TimeWithoutEating, 0f, 1f);
		
		VariableFactor TL = Fase2US.CreateVariable(m_AAnimalFase2.TimeWithoutShower, 0f, 1f);
		
		VariableFactor AnimEstablo = Fase2US.CreateVariable(m_AAnimalFase2.AnimalsOnEstable, 0f, 1f);
		
		VariableFactor AnimEspecie = Fase2US.CreateVariable(m_AAnimalFase2.ComaradesOnEstable, 0f, 1f);
		
		VariableFactor Depredadores = Fase2US.CreateVariable(m_AAnimalFase2.PredatorsOnEstable, 0f, 1f);
		
		SigmoidCurveFactor F2 = Fase2US.CreateCurve<SigmoidCurveFactor>(AnimEstablo);
		
		LinearCurveFactor Hambre = Fase2US.CreateCurve<LinearCurveFactor>(TC);
		
		LinearCurveFactor Suciedad = Fase2US.CreateCurve<LinearCurveFactor>(TL);
		
		WeightedFusionFactor Insalubridad = Fase2US.CreateFusion<WeightedFusionFactor>(Suciedad, Hambre);
		
		PointedCurveFactor F3 = Fase2US.CreateCurve<PointedCurveFactor>(AnimEspecie);
		
		ExponentialCurveFactor unnamed = Fase2US.CreateCurve<ExponentialCurveFactor>(Depredadores);
		
		WeightedFusionFactor Incomodidad = Fase2US.CreateFusion<WeightedFusionFactor>(F2, F3, unnamed);
		
		WeightedFusionFactor Infelicidad = Fase2US.CreateFusion<WeightedFusionFactor>(Insalubridad, Incomodidad);
		
		UtilityBucket AccionesDePedir = Fase2US.CreateBucket(1.3f, 0.7f);
		
		FunctionalAction Enfermar_action = new FunctionalAction();
		Enfermar_action.onStarted = m_AAnimalFase2.Enfermar;
		Enfermar_action.onUpdated = () => Status.Running;
		UtilityAction Enfermar = Fase2US.CreateAction("Enfermar", Insalubridad, Enfermar_action);//, AccionesDePedir);
		
		LinearCurveFactor Felicidad = Fase2US.CreateCurve<LinearCurveFactor>(Infelicidad);
		
		SubsystemAction MandarCorazones_action = new SubsystemAction(EstaFeliz);
		UtilityAction MandarCorazones = Fase2US.CreateAction("Felicidad", Felicidad, MandarCorazones_action);//, AccionesDePedir);
		
		FunctionalAction NecesitaLimpiarse_action = new FunctionalAction();
		NecesitaLimpiarse_action.onStarted = m_AAnimalFase2.Rascarse;
		NecesitaLimpiarse_action.onUpdated = () => Status.Running;
		UtilityAction NecesitaLimpiarse = Fase2US.CreateAction("NecesitaLimpiarse", Suciedad, NecesitaLimpiarse_action);//, AccionesDePedir);

		UtilityAction EstarIncomodo = Fase2US.CreateAction("MostrarIncomodidad", Incomodidad, null /*this action is not supported by code generation tool*/);//, AccionesDePedir);
		
		ConstantFactor Patrol = Fase2US.CreateConstant(0.5f);
		
		UtilityAction Patruyar = Fase2US.CreateAction(Patrol, null /*this action is not supported by code generation tool*/);
		
		UtilityBucket AccionesQueRepercuten = Fase2US.CreateBucket(1.3f, 0.5f);
		
		VariableFactor unnamed_1 = Fase2US.CreateVariable(m_AAnimalFase2.PuedeComer, 0f, 1f);
		
		MinFusionFactor PuedeComer = Fase2US.CreateFusion<MinFusionFactor>(unnamed_1, Hambre);
		
		SubsystemAction TieneHambre_action = new SubsystemAction(Comer);
		UtilityAction TieneHambre = Fase2US.CreateAction(PuedeComer, TieneHambre_action);//AccionesQueRepercuten

        WeightedFusionFactor unnamed_2 = Fase2US.CreateFusion<WeightedFusionFactor>(Hambre, PuedeComer);
		
		SimpleAction MostrarHambre_action = new SimpleAction();
		MostrarHambre_action.action = m_AAnimalFase2.MostrarHambre;
		UtilityAction MostrarHambre = Fase2US.CreateAction(unnamed_2, MostrarHambre_action);//, AccionesDePedir);
		
		SimpleAction Indica_que_tiene_Hambre_action = new SimpleAction();
		Indica_que_tiene_Hambre_action.action = m_AAnimalFase2.MostrarHambre;
		LeafNode Indica_que_tiene_Hambre = Comer.CreateLeafNode(Indica_que_tiene_Hambre_action);
		
		FunctionalAction GoToEat_action = new FunctionalAction();
		GoToEat_action.onUpdated = m_AAnimalFase2.MoveTowardsObjective;
		LeafNode GoToEat = Comer.CreateLeafNode(GoToEat_action);
		
		FunctionalAction Eat_action = new FunctionalAction();
		Eat_action.onUpdated = () => Status.Running;
		LeafNode Eat = Comer.CreateLeafNode(Eat_action);
		
		FleeAction Moverse_un_poco_action = new FleeAction();
		Moverse_un_poco_action.OtherTransform = Moverse_un_poco_action_OtherTransform;
		Moverse_un_poco_action.speed = 3f;
		Moverse_un_poco_action.distance = 6f;
		Moverse_un_poco_action.maxTimeRunning = 20f;
		LeafNode Moverse_un_poco = Comer.CreateLeafNode(Moverse_un_poco_action);
		
		SequencerNode EATING = Comer.CreateComposite<SequencerNode>(false, Indica_que_tiene_Hambre, GoToEat, Eat, Moverse_un_poco);
		EATING.IsRandomized = false;
		
		LoopNode unnamed_3 = Comer.CreateDecorator<LoopNode>(EATING);
		unnamed_3.Iterations = -1;
		
		SimpleAction EstaFeliz_1_action = new SimpleAction();
		EstaFeliz_1_action.action = m_AAnimalFase2.MandarCorazones;
		State EstaFeliz_1 = EstaFeliz.CreateState(EstaFeliz_1_action);
		
		PatrolAction Patrulla_action = new PatrolAction();
		Patrulla_action.maxDistance = 3f;
		State Patrulla = EstaFeliz.CreateState(Patrulla_action);
		
		UnityTimePerception _1_perception = new UnityTimePerception();
		_1_perception.TotalTime = 0.3f;
		StateTransition _1 = EstaFeliz.CreateTransition(EstaFeliz_1, Patrulla, _1_perception);
		
		UnityTimePerception _2_perception = new UnityTimePerception();
		_2_perception.TotalTime = 5f;
		StateTransition _2 = EstaFeliz.CreateTransition(Patrulla, EstaFeliz_1, _2_perception);
		
		return Fase2US;
	}
}
