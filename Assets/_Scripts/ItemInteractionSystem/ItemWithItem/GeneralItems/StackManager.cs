using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(ItemInScene))]
public class StackManager : MonoBehaviour
{
    [HideInInspector]
    public ItemInScene itemInScene;
    public UnityEvent onTaken;
    public UnityEvent onAdded;
    private void Awake()
    {
        itemInScene = GetComponent<ItemInScene>();
    }
    public void Take(int amount) //no se pueden coger mas de las que hay, habria que cambiarlo
    {
        if (amount <= 0) return;
        itemInScene.ReduceByMany(amount);
        onTaken?.Invoke();
    }
    public void Add(int amount) //se pueden añadir mas que el maximo, hBabria que cambiarlo
    {
        if (amount <= 0) return;
        itemInScene.amountInStack += amount;
        onAdded?.Invoke();
    }

}
