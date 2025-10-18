using UnityEngine;
[RequireComponent(typeof(StackManager))]

public class JoinStacks : MonoBehaviour
{
    public bool canJoinStacks = true;
    StackManager stackManager;
    private void Awake()
    {
        stackManager = GetComponent<StackManager>();
    }

    public static void JoinTwoStacks(JoinStacks baseStack, JoinStacks addedStack)
    {
        if(baseStack.GetHashCode() <= addedStack.GetHashCode()) return; //solo voy a coger un caso, que probablemente se repita al reves
        if (baseStack == null || addedStack == null) return;
        if(!baseStack.canJoinStacks || addedStack.canJoinStacks ) return;


        if (baseStack.stackManager.itemInScene.itemName != addedStack.stackManager.itemInScene.itemName) return;

        //son del mismo objettio y se puede juntar
        ItemNames itemNames = baseStack.stackManager.itemInScene.itemName;
        int maxStackSize = 10; //esto hay que preguntarlo a la base de datos

        int availableSpaceInBaseStack = maxStackSize - baseStack.stackManager.itemInScene.amountInStack;
        if (availableSpaceInBaseStack <= 0) return; //no hay espacio en la pila base
        int amountToTake = Mathf.Min(addedStack.stackManager.itemInScene.amountInStack, availableSpaceInBaseStack);
        addedStack.stackManager.Take(amountToTake);
        baseStack.stackManager.Add(amountToTake);
        return;

    }
}
