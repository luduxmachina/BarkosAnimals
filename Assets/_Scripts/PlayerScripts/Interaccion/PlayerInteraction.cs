using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField, ReadOnly] bool hasObjInHand = false;

    public void Interact()
    {
        Debug.Log("Interacting...");
        // Implement interaction logic here
    }
}
