using NUnit.Framework;
using UnityEngine;

public interface IInteractable 
{

    public bool Interact(ItemInteraction interactorType, MonoBehaviour interactor);
    
}
