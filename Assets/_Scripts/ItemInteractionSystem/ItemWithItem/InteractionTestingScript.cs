using UnityEngine;

public class InteractionTestingScript : MonoBehaviour, IInteractable
{
    public bool returnValueFromInteraction=true;
    public bool Interact(ItemInteraction interactorType, GameObject interactor)
    {
        Debug.Log( interactor.name + " interacted with me " + gameObject.name + " using " + interactorType.ToString());
        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
