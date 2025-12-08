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

public class AnimalesF2US : BehaviourRunner
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
		UtilitySystem Fase2US = new UtilitySystem(1f);
		BehaviourTree Comer = new BehaviourTree();
		FSM EstaFeliz = new FSM();
		BehaviourTree TieneHambre = new BehaviourTree();
		
		VariableFactor TC = Fase2US.CreateVariable("TC", m_AAnimalFase2.TimeWithoutEating, 0f, 1f);
		
		VariableFactor TL = Fase2US.CreateVariable("TL", m_AAnimalFase2.TimeWithoutShower, 0f, 1f);
		
		VariableFactor AnimEstablo = Fase2US.CreateVariable("AnimEstablo", m_AAnimalFase2.AnimalsOnEstable, 0f, 1f);
		
		VariableFactor AnimEspecie = Fase2US.CreateVariable("AnimEspecie", m_AAnimalFase2.ComaradesOnEstable, 0f, 1f);
		
		VariableFactor Depredadores = Fase2US.CreateVariable("Depredadores", m_AAnimalFase2.PredatorsOnEstable, 0f, 1f);
		
		SigmoidCurveFactor F2 = Fase2US.CreateCurve<SigmoidCurveFactor>("F2", AnimEstablo);
		F2.GrownRate = 6f;
		F2.Midpoint = 0.8f;
		
		LinearCurveFactor Hambre = Fase2US.CreateCurve<LinearCurveFactor>("Hambre", TC);
		Hambre.Slope = 0.8f;
        Hambre.YIntercept = 0f;


        LinearCurveFactor Suciedad = Fase2US.CreateCurve<LinearCurveFactor>("Suciedad", TL);
		Suciedad.Slope = 0.7f;
		Suciedad.YIntercept = 0f;

        PointedCurveFactor F3 = Fase2US.CreateCurve<PointedCurveFactor>("F3", AnimEspecie);
		F3.Points = new List<CurvePoint>() { new CurvePoint(0f, 0.6f), new CurvePoint(0.5f, 0f), new CurvePoint(0f, 0.6f) };

        ExponentialCurveFactor F4 = Fase2US.CreateCurve<ExponentialCurveFactor>("F4", Depredadores);
		F4.Exponent = 0.5f;
		F4.DespX = 0f;
		F4.DespY = 0.3f;

        WeightedFusionFactor Incomodidad = Fase2US.CreateFusion<WeightedFusionFactor>("Incomodidad", F2, F3, F4);
		Incomodidad.Weights = new float[] { 0.25f, 0.25f, 0.5f };
		
		FunctionalAction NecesitaLimpiarse_action = new FunctionalAction();
		NecesitaLimpiarse_action.onStarted = m_AAnimalFase2.Rascarse;
		NecesitaLimpiarse_action.onUpdated = () => Status.Running;
		UtilityAction NecesitaLimpiarse = Fase2US.CreateAction("NecesitaLimpiarse", Suciedad, NecesitaLimpiarse_action);



		SimpleAction mostrarIncomodo_action = new SimpleAction();
        mostrarIncomodo_action.action = m_AAnimalFase2.MostrarIncomodidad;

        PatrolAction moverseIncomodo_action = new PatrolAction();
		moverseIncomodo_action.maxDistance = 10f;

        SequenceAction estarIncomodo_Parallel_action = new SequenceAction(Status.Success, mostrarIncomodo_action, moverseIncomodo_action );

        UtilityAction EstarIncomodo = Fase2US.CreateAction("EstarIncomodo", Incomodidad, estarIncomodo_Parallel_action);
		
		ConstantFactor Patrol = Fase2US.CreateConstant("Patrol", 0.5f);



        SimpleAction patrullarNoMostrarNada_action = new SimpleAction();
        patrullarNoMostrarNada_action.action = m_AAnimalFase2.NoMostrarNada;

        PatrolAction patrullarMoverse_action = new PatrolAction();
        patrullarMoverse_action.maxDistance = 5f;

        SequenceAction patrullar_Parallel_action = new SequenceAction(Status.Success, patrullarNoMostrarNada_action, patrullarMoverse_action);
        UtilityAction Patruyar = Fase2US.CreateAction("Patrullar", Patrol, patrullar_Parallel_action);
		
		VariableFactor hayComida = Fase2US.CreateVariable(m_AAnimalFase2.PuedeComer, 0f, 1f);
		
		MinFusionFactor PuedeComer = Fase2US.CreateFusion<MinFusionFactor>("PuedeComer", hayComida, Hambre);
		
		SubsystemAction TieneHambreYPuedeComer_action = new SubsystemAction(Comer);
		UtilityAction TieneHambreYPuedeComer = Fase2US.CreateAction("TieneHambreYPuedeComer", PuedeComer, TieneHambreYPuedeComer_action);
		
		WeightedFusionFactor HambreComidaFusion = Fase2US.CreateFusion<WeightedFusionFactor>(Hambre, PuedeComer);
        HambreComidaFusion.Weights = new float[] { 1.0f, -1.0f };

        SubsystemAction MostrarHambre_action = new SubsystemAction(TieneHambre, true);
		UtilityAction MostrarHambre = Fase2US.CreateAction("MostrarHambre", HambreComidaFusion, MostrarHambre_action);


        WeightedFusionFactor Insalubridad = Fase2US.CreateFusion<WeightedFusionFactor>("Insalubridad", Suciedad, HambreComidaFusion);
        Insalubridad.Weights = new float[] { 0.4f, 0.7f };

        WeightedFusionFactor Infelicidad = Fase2US.CreateFusion<WeightedFusionFactor>("Infelicidad", Insalubridad, Incomodidad);
        Infelicidad.Weights = new float[] { 0.7f, 0.3f };

        FunctionalAction Enfermar_action = new FunctionalAction();
        Enfermar_action.onStarted = m_AAnimalFase2.Enfermar;
        Enfermar_action.onUpdated = () => Status.Running;
		Enfermar_action.onStopped = m_AAnimalFase2.YaNoEstaEnfermo;
		Enfermar_action.onPaused = m_AAnimalFase2.YaNoEstaEnfermo;
        UtilityAction Enfermar = Fase2US.CreateAction("Enfermar", Insalubridad, Enfermar_action);

        LinearCurveFactor Felicidad = Fase2US.CreateCurve<LinearCurveFactor>("Felicidad", Infelicidad);
        Felicidad.Slope = -0.7f;
        Felicidad.YIntercept = 0.7f;

        SubsystemAction MandarCorazones_action = new SubsystemAction(EstaFeliz);
        UtilityAction MandarCorazones = Fase2US.CreateAction("MandarCorazones", Felicidad, MandarCorazones_action);

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

        SimpleAction EstaFeliz_1_action = new SimpleAction();
		EstaFeliz_1_action.action = m_AAnimalFase2.MandarCorazones;
		State EstaFeliz_1 = EstaFeliz.CreateState("EstaFeliz", EstaFeliz_1_action);
		
		PatrolAction Patrulla_action = new PatrolAction();
		Patrulla_action.maxDistance = 3f;
		State Patrulla = EstaFeliz.CreateState("Patrulla", Patrulla_action);
		
		UnityTimePerception _1_perception = new UnityTimePerception();
		_1_perception.TotalTime = 0.3f;
		StateTransition _1 = EstaFeliz.CreateTransition("1", EstaFeliz_1, Patrulla, _1_perception);
		
		UnityTimePerception _2_perception = new UnityTimePerception();
		_2_perception.TotalTime = 5f;
		StateTransition _2 = EstaFeliz.CreateTransition("2", Patrulla, EstaFeliz_1, _2_perception);

        //el root no es tan imnportante en FSM pero asi empieza en el sitio correcto
        EstaFeliz.SetEntryState(EstaFeliz_1);

        SimpleAction MostrarHambre_1_action = new SimpleAction();
		MostrarHambre_1_action.action = m_AAnimalFase2.MostrarHambre;
		LeafNode MostrarHambre_1 = TieneHambre.CreateLeafNode("MostrarHambre", MostrarHambre_1_action);
		
		PatrolAction unnamed_4_action = new PatrolAction();
		unnamed_4_action.maxDistance = 10f;
		LeafNode unnamed_4 = TieneHambre.CreateLeafNode(unnamed_4_action);
		
		SequencerNode SecuenciaPrincipal = TieneHambre.CreateComposite<SequencerNode>("SecuenciaPrincipal", false, MostrarHambre_1, unnamed_4);
		SecuenciaPrincipal.IsRandomized = false;


        //el root es importante para los arboles
        TieneHambre.SetRootNode(SecuenciaPrincipal);

        if (useDebugger)
        {

            debuggerComponent.RegisterGraph(Fase2US);
            debuggerComponent.RegisterGraph(Comer);
            debuggerComponent.RegisterGraph(EstaFeliz);
            debuggerComponent.RegisterGraph(TieneHambre);

        }

        return Fase2US;
	}
}
