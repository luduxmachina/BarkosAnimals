using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.StateMachines.StackFSMs;

using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public class TotemBirdComportamiento : BehaviourRunner
{

    [SerializeField] private bool useDebugger = false;
    [SerializeField, HideIf("useDebugger", false)] private BSRuntimeDebugger debuggerComponent;

    [SerializeField] private TotemAnimal m_TotemAnimal;

	private IMovementComponent movementComponent;

    private PushPerception Cogido;
	private PushPerception Soltado;
	
	protected override void Init()
	{
		m_TotemAnimal = GetComponent<TotemAnimal>();
        movementComponent = GetComponent<IMovementComponent>();
        base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		StackFSM MainGraphTotemBird = new StackFSM();
		StackFSM SubGraphPajaroLibre = new StackFSM();
		
		SubsystemAction PajaroLibre_action = new SubsystemAction(SubGraphPajaroLibre);
		State PajaroLibre = MainGraphTotemBird.CreateState("PajaroLibre", PajaroLibre_action);
		
		SimpleAction PajaroCogido_action = new SimpleAction();
		PajaroCogido_action.action = m_TotemAnimal.Cogido;
		State PajaroCogido = MainGraphTotemBird.CreateState("PajaroCogido", PajaroCogido_action);
		
		StateTransition Atrapado = MainGraphTotemBird.CreateTransition("Atrapado", PajaroLibre, PajaroCogido, statusFlags: StatusFlags.None);
		
		StateTransition NoAtrapado = MainGraphTotemBird.CreateTransition("NoAtrapado", PajaroCogido, PajaroLibre, statusFlags: StatusFlags.None);
		
		PathingActionTransformParent PatruyandoNido_action = new PathingActionTransformParent();
		PatruyandoNido_action.wayPointsParent = m_TotemAnimal.nido;
		
		PatruyandoNido_action.distanceThreshold = m_TotemAnimal.GetAttackRange();
		State PatrullandoNido = SubGraphPajaroLibre.CreateState("PatrullandoNido", PatruyandoNido_action);
        StateTransition TerinaPatrulla = SubGraphPajaroLibre.CreateTransition("TerminaPatrulla", PatrullandoNido, PatrullandoNido, statusFlags: StatusFlags.Finished);

        SimpleAction cogerPan= new SimpleAction();
        cogerPan.action = m_TotemAnimal.CogerComida	;
		FunctionalAction dejarPanEnNido = new FunctionalAction();
        dejarPanEnNido.onStarted = m_TotemAnimal.LlevarComidaInit;
        dejarPanEnNido.onUpdated = m_TotemAnimal.LlevarComidaUpdate;

        SequenceAction RecogerPanSequence = new SequenceAction(Status.Success, cogerPan, dejarPanEnNido);

        State RecogerPan = SubGraphPajaroLibre.CreateState("RecogerPan", RecogerPanSequence);
		
		FunctionalAction AcercarseAPan_action = new FunctionalAction();
		AcercarseAPan_action.onStarted = m_TotemAnimal.MoveTowardsObjectiveInit;
		AcercarseAPan_action.onUpdated = m_TotemAnimal.MoveTowardsObjective;
		State AcercarseAPan = SubGraphPajaroLibre.CreateState("AcercarseAPan", AcercarseAPan_action);
		
		ConditionPerception VePan_perception = new ConditionPerception();
		VePan_perception.onCheck = m_TotemAnimal.ObjectiveInSight;
		StateTransition VePan = SubGraphPajaroLibre.CreateTransition("VePan", PatrullandoNido, AcercarseAPan, VePan_perception);
		
		ConditionPerception PanAlcanzado_perception = new ConditionPerception();
		PanAlcanzado_perception.onCheck = m_TotemAnimal.ObjectiveCloseToAttack;
		StateTransition PanAlcanzado = SubGraphPajaroLibre.CreateTransition("PanAlcanzado", AcercarseAPan, RecogerPan, PanAlcanzado_perception);
		
		StateTransition FinDejarComida = SubGraphPajaroLibre.CreateTransition("FinDejarComida", RecogerPan, PatrullandoNido, statusFlags: StatusFlags.Finished);
		
		StateTransition NoPanAlcanzado = SubGraphPajaroLibre.CreateTransition("NoPanAlcanzado", AcercarseAPan, PatrullandoNido, statusFlags: StatusFlags.Failure);
		
		PatrolActionGivenCenter EsconderseEnNido_action = new PatrolActionGivenCenter();
		EsconderseEnNido_action.maxDistance = 0.3f;
		EsconderseEnNido_action.centerTransform = m_TotemAnimal.nido;
		State EsconderseEnNido = SubGraphPajaroLibre.CreateState("EsconderseEnNido", EsconderseEnNido_action);
		
		ConditionPerception Ve_peligro_push_perception = new ConditionPerception();
		Ve_peligro_push_perception.onCheck = m_TotemAnimal.PredatorInSight;

        PushTransition Ve_peligro_push = SubGraphPajaroLibre.CreatePushTransition("Ve peligro push", AcercarseAPan, EsconderseEnNido, Ve_peligro_push_perception);
		Ve_peligro_push.Action = new SimpleAction( () => { this.movementComponent.Speed = m_TotemAnimal.GetRunSpeed(); });
        

        ConditionPerception VePeligroPush_perception = new ConditionPerception();
		VePeligroPush_perception.onCheck = m_TotemAnimal.PredatorInSight;
		PushTransition VePeligroPush = SubGraphPajaroLibre.CreatePushTransition("VePeligro Push", PatrullandoNido, EsconderseEnNido, VePeligroPush_perception);
        VePeligroPush.Action = new SimpleAction(() => { this.movementComponent.Speed = m_TotemAnimal.GetRunSpeed(); });

        ConditionPerception VePeligroPush_1_perception = new ConditionPerception();
		VePeligroPush_1_perception.onCheck = m_TotemAnimal.PredatorInSight;
		PushTransition VePeligroPush_1 = SubGraphPajaroLibre.CreatePushTransition("VePeligroPush", RecogerPan, EsconderseEnNido, VePeligroPush_1_perception);
        VePeligroPush_1.Action = new SimpleAction(() => { this.movementComponent.Speed = m_TotemAnimal.GetRunSpeed(); });

        DelayAction EsperarEscondido_action = new DelayAction();
		EsperarEscondido_action.delayTime = 3f;
		State EsperarEscondido = SubGraphPajaroLibre.CreateState("EsperarEscondido", EsperarEscondido_action);
		
		StateTransition unnamed = SubGraphPajaroLibre.CreateTransition(EsconderseEnNido, EsperarEscondido, statusFlags: StatusFlags.Finished);
		
		PopTransition VolverAloQueSeEstuviera = SubGraphPajaroLibre.CreatePopTransition("VolverAloQueSeEstuviera", EsperarEscondido, statusFlags: StatusFlags.Finished);
        VolverAloQueSeEstuviera.Action = new SimpleAction(() => { this.movementComponent.Speed = m_TotemAnimal.GetWalkingSpeed(); });


        SubGraphPajaroLibre.SetEntryState(PatrullandoNido);
        MainGraphTotemBird.SetEntryState(PajaroLibre);

        Cogido = new PushPerception(Atrapado);
		Soltado = new PushPerception(Atrapado, NoAtrapado);
        if (useDebugger)
        {

            debuggerComponent.RegisterGraph(MainGraphTotemBird);
            debuggerComponent.RegisterGraph(SubGraphPajaroLibre);
        }
        return MainGraphTotemBird;
	}
	public void NotifyCogido()
    {
        Cogido.Fire();
    }
    public void NotifySoltado()
    {
        Soltado.Fire();
    }
}
