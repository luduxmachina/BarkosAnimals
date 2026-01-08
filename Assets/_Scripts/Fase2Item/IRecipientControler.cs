using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UnityToolkit.GUIDesigner.Framework;
using UnityEngine;
using UnityEngine.Events;

public abstract class IRecipientControler: SmartObject
{
    public abstract void SubscribeStable(Stable stable);
    //public abstract BehaviourTree CreateGraph(AAnimalFase2 animalFase2);
    public abstract Transform GetTransfToEat(AAnimalFase2 animal);
    public abstract bool ComederoLibre(AAnimalFase2 animal);
    public abstract bool HayComida(ItemNames[] tiposComida);
    public abstract bool AddStack(ItemNames tipoComida);
    public abstract bool RemoveStack(ItemNames[] tiposComida, AAnimalFase2 animal);

}
