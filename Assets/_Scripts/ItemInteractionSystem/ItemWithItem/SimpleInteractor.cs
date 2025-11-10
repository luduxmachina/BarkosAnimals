using UnityEngine;
using UnityEngine.Events;

public class SimpleInteractor : MonoBehaviour
{
    [SerializeField] LazyDetector detector; //vaya puta mierda que no se puedan poner interfaces en el inspector
    [SerializeField, Tooltip("Por si el objeto que dices que interactua no es este")] GameObject interactingObject;
    [Header("Interaction")]
    [SerializeField]
    private bool useSpecificInteractionType = false;
    [SerializeField, HideIf("useSpecificInteractionType", false)]
    public ItemNames interactionType = ItemNames.None;
    public UnityEvent OnInteraction;
    public UnityEvent OnFailedInteraction;

    private InteractsWithPlayer playerReceiver;
    private void Awake()
    {
        playerReceiver = GetComponent<InteractsWithPlayer>();
        if (detector == null)
        {
            detector = GetComponent<LazyDetector>();
            if (detector == null)
            {
                Debug.LogWarning("No detector found, disabling SimpleInteractor.");
                this.enabled = false;
            }
        }
    }

    public void InteractWithTarget()
    {
        var target = GetTarget();
        if (target == null) { return; }
        ItemNames sentInteraction= ItemNames.None;

        if (useSpecificInteractionType)
        {
            sentInteraction = interactionType;
        }
        else
        {
            var itemInScene = interactingObject != null ? interactingObject.GetComponent<ItemInScene>() : this.GetComponent<ItemInScene>();
            if (itemInScene != null)
            {
                sentInteraction = itemInScene.itemName;
            }
        }

        GameObject interactor= (interactingObject != null) ? interactingObject : this.gameObject;


        bool success= target.Interact(sentInteraction, interactor);
        if (playerReceiver != null)
        {
            playerReceiver.interactionSuccessful = success;
        }
        if (success)
        {
            OnInteraction?.Invoke();
        }
        else
        {
            OnFailedInteraction?.Invoke();
        }
        

    }
    protected IInteractable GetTarget()
    {
        GameObject targetGO = detector.GetTarget();
        if (targetGO == null) { return null; }
        var target = targetGO.GetComponent<IInteractable>();
        return target;
    }
    private void OnEnable()
    {
        if (playerReceiver != null)
        {
            playerReceiver.OnPlayerInteract.AddListener(() =>
            {
                InteractWithTarget();
            });
        }
    }
    private void OnDisable()
    {
        if (playerReceiver != null)
        {
            playerReceiver.OnPlayerInteract.RemoveListener(() =>
            {
                InteractWithTarget();
            });
        }
    }
}
