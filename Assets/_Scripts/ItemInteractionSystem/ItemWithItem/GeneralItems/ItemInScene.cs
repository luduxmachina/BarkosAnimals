using UnityEngine;

public class ItemInScene : MonoBehaviour
{
    [SerializeField] private GameObject itemParent;
    [Tooltip("Cuantos items de este tipo hay en este objeto")]
    public int amountInStack = 1;
    public ItemNames itemName;
    [SerializeField, ReadOnly]
    ItemType itemType; //preguntar a fran por esto

    [Header("Other scripts")]
    [SerializeField]
    private bool useParentGrabbable = true;
    [SerializeField, HideIf("useParentGrabbable", true)]
    private SimpleGrabbable simpleGrabbable;

    public void GetInCart(int leftOver)
    {
        amountInStack = leftOver;
        //avisar a UI???
        CheckStatus();

    }
    public void ReduceByMany(int amountToReduce)
    {
        amountInStack -= amountToReduce;
        CheckStatus();
    }
    public void ReduceByOne()
    {
        //Debug.Log("Se ha comido un trozo");
        amountInStack--;
        CheckStatus();
    }
    private void CheckStatus()
    {
        if (amountInStack > 0) return;
       // Debug.Log("Item " + gameObject.name + " getting in cart");
        GetGrabbable()?.Drop();

        Destroy(itemParent);
    }
    private IGrabbable GetGrabbable()
    {
        if (!useParentGrabbable) return simpleGrabbable;
        return GetComponentInParent<IGrabbable>();
    }
    private void Start()
    {
        IslandPositions.instance?.AddPosition(itemName, itemParent.transform);
    }
    private void OnDisable()
    {
        IslandPositions.instance?.RemovePosition(itemName, itemParent.transform);
    }
}
