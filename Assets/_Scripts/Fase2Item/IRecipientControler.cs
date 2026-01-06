using UnityEngine;
using UnityEngine.Events;
using BehaviourAPI.Core;

public interface IRecipientControler
{
    public void SubscribeStable(Stable stable);
    public BehaviourGraph CreateGraph(AAnimalFase2 animalFase2);
    public Transform GetTransfToEat(AAnimalFase2 animal);
    public bool ComederoLibre(AAnimalFase2 animal);
    public bool HayComida(ItemNames[] tiposComida);
    public bool RemoveStack(ItemNames[] tiposComida, AAnimalFase2 animal);

}
