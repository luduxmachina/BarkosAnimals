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

public class SerpienteBehaviour : BehaviourRunner
{
	[SerializeField] private SerpienteInScene m_SerpienteInScene;
	[SerializeField] private BSRuntimeDebugger debuggerComponent;
    [SerializeField] private Transform HuirRecto_action_OtherTransform;
	[SerializeField] private Transform HuirIzq_action_OtherTransform;
	private PushPerception Cogido;
	private float runSpeed;
	private float runAwayDistance;
	protected override void Init()
	{	if(m_SerpienteInScene == null)
		{
            m_SerpienteInScene = GetComponent<SerpienteInScene>();

        }
        runSpeed = m_SerpienteInScene.GetRunSpeed();
        runAwayDistance = m_SerpienteInScene.GetRadioAwareness()*1.25f;

        base.Init();
	}
	public void OnCogido()
    {
        Cogido.Fire();
    }
    protected override BehaviourGraph CreateGraph()
	{
		FSM SnakeFSM = new FSM();
		BehaviourTree SnakeTranquiCazandoBT = new BehaviourTree();
		BehaviourTree SnakeHuyendoBT = new BehaviourTree();
		
		SubsystemAction TranquiloCazando_action = new SubsystemAction(SnakeTranquiCazandoBT);
		TranquiloCazando_action.ExecuteOnLoop = true;
        State TranquiloCazando = SnakeFSM.CreateState("TranquiloCazando",TranquiloCazando_action);
		
		SubsystemAction Huyendo_action = new SubsystemAction(SnakeHuyendoBT);
        TranquiloCazando_action.ExecuteOnLoop = true;

        ParallelAction parallelHuyendo = new ParallelAction(false, false, Huyendo_action, new SimpleAction(m_SerpienteInScene.PlayRunAnim));
        //State Huyendo = SnakeFSM.CreateState("Huyendo", Huyendo_action);
        State Huyendo = SnakeFSM.CreateState("Huyendo",parallelHuyendo);
		
		SimpleAction AtacarJugador_action = new SimpleAction();
		AtacarJugador_action.action = m_SerpienteInScene.AttackGrabber;
		State AtacarJugador = SnakeFSM.CreateState("AtacarJugador", AtacarJugador_action);
		
		StateTransition SerCogido = SnakeFSM.CreateTransition(Huyendo, AtacarJugador, statusFlags: StatusFlags.None);
		
		UnityTimePerception TiempoTrasAtacar_perception = new UnityTimePerception();
		TiempoTrasAtacar_perception.TotalTime = m_SerpienteInScene.GetTiempoDescanso();
		StateTransition TiempoTrasAtacar = SnakeFSM.CreateTransition(AtacarJugador, Huyendo, TiempoTrasAtacar_perception);
		
		ConditionPerception PeligroCerca_perception = new ConditionPerception();
		PeligroCerca_perception.onCheck = m_SerpienteInScene.PredatorClose;
		SimpleAction PeligroCerca_action = new SimpleAction();
		PeligroCerca_action.action = m_SerpienteInScene.PlaySurpriseAnim;
		StateTransition PeligroCerca = SnakeFSM.CreateTransition(TranquiloCazando, Huyendo, PeligroCerca_perception, PeligroCerca_action);
		
		ConditionPerception PeligroYaLejos_perception = new ConditionPerception();
		PeligroYaLejos_perception.onCheck = m_SerpienteInScene.NotPredatorClose;
		StateTransition PeligroYaLejos = SnakeFSM.CreateTransition(Huyendo, TranquiloCazando, PeligroYaLejos_perception);
		
		FunctionalAction Acechar_action = new FunctionalAction();
		Acechar_action.onStarted = m_SerpienteInScene.MoveTowardsObjectiveInit;
		Acechar_action.onUpdated = m_SerpienteInScene.MoveTowardsObjective;
		LeafNode Acechar = SnakeTranquiCazandoBT.CreateLeafNode("Acechar",Acechar_action);
		
		ConditionNode Hay_PresaCerca = SnakeTranquiCazandoBT.CreateDecorator<ConditionNode>("HayPresaCerca",Acechar);
		ConditionPerception presaCercaPerception= new ConditionPerception();
        presaCercaPerception.onCheck = m_SerpienteInScene.ObjectiveClose;
		Hay_PresaCerca.Perception = presaCercaPerception;

        SimpleAction ComerCarro_action = new SimpleAction();
		ComerCarro_action.action = m_SerpienteInScene.ComerEnCarro;
		LeafNode ComerCarro = SnakeTranquiCazandoBT.CreateLeafNode("COmerdel carro",ComerCarro_action);
		
		ConditionNode ObjetivoEsCarro = SnakeTranquiCazandoBT.CreateDecorator<ConditionNode>("Objetivo es carro", ComerCarro);
        ConditionPerception objetivoEsCarroPerception = new ConditionPerception();
        objetivoEsCarroPerception.onCheck = m_SerpienteInScene.ObjectiveIsCart;
        ObjetivoEsCarro.Perception = objetivoEsCarroPerception;


        FunctionalAction ComerNormal_action = new FunctionalAction();
		ComerNormal_action.onStarted = m_SerpienteInScene.InitComer;
		ComerNormal_action.onUpdated = m_SerpienteInScene.UpdateComer;
		LeafNode ComerNormal = SnakeTranquiCazandoBT.CreateLeafNode("ComerNormal", ComerNormal_action);
		
		SelectorNode ComerOMeterseAlCarro = SnakeTranquiCazandoBT.CreateComposite<SelectorNode>(false, ObjetivoEsCarro, ComerNormal);
		ComerOMeterseAlCarro.IsRandomized = false;
		
		ConditionNode PresaARangoDeComida = SnakeTranquiCazandoBT.CreateDecorator<ConditionNode>("PresaARango de comida", ComerOMeterseAlCarro);
        ConditionPerception presaARangoDeComidaPerception = new ConditionPerception();
        presaARangoDeComidaPerception.onCheck = m_SerpienteInScene.ObjectiveCloseToAttack;
        PresaARangoDeComida.Perception = presaARangoDeComidaPerception;

        SequencerNode Cazando = SnakeTranquiCazandoBT.CreateComposite<SequencerNode>(false, Hay_PresaCerca, PresaARangoDeComida);
		Cazando.IsRandomized = false;
		
		PatrolAction Patrullar_action = new PatrolAction();
	
		Patrullar_action.maxDistance = m_SerpienteInScene.GetPatrolRadius();
		ParallelAction parallelPatrullar= new ParallelAction(false, true, Patrullar_action, new SimpleAction(m_SerpienteInScene.PlayWalkAnim));

        //LeafNode Patrullar = SnakeTranquiCazandoBT.CreateLeafNode("Patrullar", Patrullar_action);
        LeafNode Patrullar = SnakeTranquiCazandoBT.CreateLeafNode("Patrullar", parallelPatrullar);
		
		SelectorNode TranquiOCazando = SnakeTranquiCazandoBT.CreateComposite<SelectorNode>(false, Cazando, Patrullar);
		TranquiOCazando.IsRandomized = false;
		
		LoopNode RootTranquilo = SnakeTranquiCazandoBT.CreateDecorator<LoopNode>(TranquiOCazando);
		RootTranquilo.Iterations = -1;

        SnakeTranquiCazandoBT.SetRootNode(RootTranquilo);

        FleeActionNoRecto HuirRecto_action = new FleeActionNoRecto();
		HuirRecto_action.animalContext = m_SerpienteInScene;
        HuirRecto_action.OtherTransform = HuirRecto_action_OtherTransform;
		HuirRecto_action.speed = runSpeed;
		HuirRecto_action.distance = runAwayDistance;
		HuirRecto_action.maxTimeRunning = 2f;
		LeafNode HuirRecto = SnakeHuyendoBT.CreateLeafNode("Huirrecto", HuirRecto_action);
		
		FleeActionNoRecto HuirIzq_action = new FleeActionNoRecto();
        HuirIzq_action.animalContext = m_SerpienteInScene;

        HuirIzq_action.angulo = -30f;
		HuirIzq_action.OtherTransform = HuirIzq_action_OtherTransform;
		HuirIzq_action.speed = runSpeed;
		HuirIzq_action.distance = runAwayDistance;
		HuirIzq_action.maxTimeRunning = 2f;
		LeafNode HuirIzq = SnakeHuyendoBT.CreateLeafNode("HuirIzq", HuirIzq_action);
		
		FleeActionNoRecto HuirDer_action = new FleeActionNoRecto();
		HuirDer_action.animalContext = m_SerpienteInScene;
        HuirDer_action.angulo = 30f;
		HuirDer_action.OtherTransform = HuirRecto_action_OtherTransform;
		HuirDer_action.speed = runSpeed;
		HuirDer_action.distance = runAwayDistance;
		HuirDer_action.maxTimeRunning = 2f;
		LeafNode HuirDer = SnakeHuyendoBT.CreateLeafNode("HuirDer", HuirDer_action);
		
		SequencerNode MoversePorLosLados = SnakeHuyendoBT.CreateComposite<SequencerNode>(true, HuirIzq, HuirDer);
		MoversePorLosLados.IsRandomized = true;
		
		SelectorNode MovimientoAleatorio = SnakeHuyendoBT.CreateComposite<SelectorNode>(true, HuirRecto, MoversePorLosLados);
		MovimientoAleatorio.IsRandomized = true;
		
		LoopNode rootHuyendo = SnakeHuyendoBT.CreateDecorator<LoopNode>(MovimientoAleatorio);
		rootHuyendo.Iterations = -1;
        SnakeHuyendoBT.SetRootNode(rootHuyendo);

        Cogido = new PushPerception(SerCogido);
		debuggerComponent.RegisterGraph(SnakeTranquiCazandoBT);
		debuggerComponent.RegisterGraph(SnakeHuyendoBT);
        debuggerComponent.RegisterGraph(SnakeFSM);
		return SnakeFSM;
	}
}
