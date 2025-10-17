using NUnit.Framework;
using UnityEngine;

public interface IInteractable 
{

    public bool Interact(ItemNames interactorType, GameObject interactor);
    
}
