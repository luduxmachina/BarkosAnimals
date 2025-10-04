using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    public UnityEvent<ItemInteraction> OnInteract;





    public virtual void Interact(ItemInteraction code)
    {     
      OnInteract?.Invoke(code);
    }



}