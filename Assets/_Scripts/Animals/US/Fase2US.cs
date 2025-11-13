using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UtilitySystems;
using BehaviourAPI.UnityToolkit.GUIDesigner.Framework;

public class Fase2US : BehaviourRunner
{
	[SerializeField] private AAnimal m_Animal;
	
	protected override void Init()
	{
		m_Animal = GetComponent<PatoFase1>();
		
		base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		var Fase2US = new UtilitySystem(1.3f);
		
		var TC = Fase2US.CreateVariable("TC", m_Animal.GetPredatorsOnEstable, 0f, 1f);
		
		var TL = Fase2US.CreateVariable("TL", m_Animal.GetTimeWithoutShower, 0f, 1f);
		
		var AnimEstablo = Fase2US.CreateVariable("AnimEstablo", m_Animal.GetAnimalsOnEstable, 0f, 1f);
		
		var AnimEspecie = Fase2US.CreateVariable("AnimEspecie", m_Animal.GetPredatorsOnEstable, 0f, 1f);
		
		var Depredadores = Fase2US.CreateVariable("Depredadores", m_Animal.GetPredatorsOnEstable, 0f, 1f);
		
		var HayComida = Fase2US.CreateVariable("HayComida", m_Animal.TieneComida, 0f, 1f);
		
		var F3 = Fase2US.CreateCurve<ExponentialCurveFactor>("F3", AnimEspecie);
		
		var F2 = Fase2US.CreateCurve<SigmoidCurveFactor>("F2", AnimEstablo);
		
		var Hambre = Fase2US.CreateCurve<LinearCurveFactor>("Hambre", TC);
		
		var F1 = Fase2US.CreateFusion<WeightedFusionFactor>("F1", HayComida, Hambre);
		
		var Limpieza = Fase2US.CreateCurve<LinearCurveFactor>("Limpieza", TL);
		
		var Salud = Fase2US.CreateFusion<WeightedFusionFactor>("Salud", Hambre, Limpieza);
		
		var F4 = Fase2US.CreateCurve<LinearCurveFactor>("F4", Depredadores);
		
		var Comodidad = Fase2US.CreateFusion<WeightedFusionFactor>("Comodidad", F2, F3, F4);
		
		var Felicidad = Fase2US.CreateFusion<WeightedFusionFactor>("Felicidad", Salud, Comodidad);
		
		var Comer_action = new CustomAction();
		var Comer = Fase2US.CreateAction("Comer", F1, Comer_action);
		
		//var Enfermar_action = new WalkAction();
		//Enfermar_action.Target = new Vector3(10f, 1f, 1f);
		//var Enfermar = Fase2US.CreateAction("Enfermar", Salud, Enfermar_action);
		//
		//var MandarCorazones_action = new PatrolAction();
		//MandarCorazones_action.maxDistance = 10f;
		//var MandarCorazones = Fase2US.CreateAction("MandarCorazones", Felicidad, MandarCorazones_action);
		//
		//var Rascarse_action = new SimpleAction();
		//Rascarse_action.action = m_Animal.Aletear;
		//var Rascarse = Fase2US.CreateAction("Rascarse", Limpieza, Rascarse_action);
		//
		//var EstarIncomodo_action = new PatrolAction();
		//EstarIncomodo_action.maxDistance = 10f;
		//var EstarIncomodo = Fase2US.CreateAction("EstarIncomodo", Comodidad, EstarIncomodo_action);
		//
		return Fase2US;
	}
}
