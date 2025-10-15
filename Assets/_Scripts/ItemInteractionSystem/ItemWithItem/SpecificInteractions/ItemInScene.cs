using UnityEngine;

public class ItemInScene : MonoBehaviour
{
    [Tooltip("Cuantos items de este tipo hay en este objeto")]
    public int amountInStack = 1;
    public ItemNames itemName;
    [SerializeField, ReadOnly]
    ItemType itemType; //preguntar a fran por esto

    public void GetInCart(int leftOver)
    {
        amountInStack = leftOver;
        //avisar a UI???
        if (amountInStack > 0) return;
        Debug.Log("Item "+ gameObject.name+ " getting in cart");
        GetComponent<SimpleGrabbable>()?.Drop();
        Destroy(gameObject);
    }
}
