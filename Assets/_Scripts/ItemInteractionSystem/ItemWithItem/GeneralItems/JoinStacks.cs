using UnityEngine;
[RequireComponent(typeof(StackManager))]

public class JoinStacks : MonoBehaviour
{
    public bool canJoinStacks = true;
    StackManager stackManager;
    [SerializeField]
    CompleteTaggedDetector detector;
    private void Awake()
    {
        stackManager = GetComponent<StackManager>();
    }
    private void Update()
    {
        if (detector == null) return;
        if (!canJoinStacks) return;
        if (!detector.HasTarget()) return;
        foreach (GameObject detected in detector.GetAllTargets())
        {
            JoinStacks otherStack = detected.GetComponentInChildren<JoinStacks>();
            if (otherStack != null)
            {
                Debug.Log("Joining stacks");
                JoinTwoStacks(this, otherStack);
                return; //total se hace cada frame
            }
        }
    }

    public static void JoinTwoStacks(JoinStacks baseStack, JoinStacks addedStack)
    {
        Debug.Log("Trying to join stacks");
        if (baseStack.GetHashCode() <= addedStack.GetHashCode()) return; //solo voy a coger un caso, que probablemente se repita al reves
        Debug.Log("Passed hashcode check");
        if (baseStack == null || addedStack == null) return;
        if(!baseStack.canJoinStacks || !addedStack.canJoinStacks ) return;


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
