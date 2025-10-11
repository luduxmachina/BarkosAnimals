using UnityEngine;

public class GetInCartHandler : MonoBehaviour
{
    
    public void GetInCart()
    {
        Debug.Log("Item "+ gameObject.name+ " getting in cart");
        GetComponent<SimpleGrabbable>()?.Drop();
        Destroy(gameObject);
    }
}
