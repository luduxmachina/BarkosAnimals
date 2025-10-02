using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent<int> OnInteract;



    public virtual void Interact(int code)
    {
      OnInteract?.Invoke(code);
    }
   


}