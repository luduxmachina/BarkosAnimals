using UnityEngine;
using UnityEngine.Events;

public class FoodEater : MonoBehaviour, IInteractable
{
    public ItemNames acceptedFoodType = ItemNames.Bread;
    public bool wantsFood = true;
    public bool stopsBeingHungryOnEat = true;
    public UnityEvent OnEat;
    public bool Interact(ItemNames interactorType, GameObject interactor)
    {
        if (!wantsFood) { return false; }
        if (interactorType != acceptedFoodType)
        {
            return false;
        }
        ItemInScene itemInScene = interactor.GetComponentInChildren<ItemInScene>();
        if (itemInScene != null )
        {
            itemInScene.ReduceByOne();
        }
        //si es la comida que come
        if (stopsBeingHungryOnEat)
        {
            wantsFood = false;
        }
        OnEat?.Invoke();

        return true;
    }
}
