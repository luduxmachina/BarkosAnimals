using UnityEngine;
using UnityEngine.Events;

public class SimpleInteractor : MonoBehaviour
{
    [SerializeField] LazyDetector detector; //vaya puta mierda que no se puedan poner interfaces en el inspector

    [Header("Interaction")]
    public ItemInteraction interactionType = ItemInteraction.testing1;
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
        GameObject targetGO = detector.GetTarget();
        if (targetGO == null ) { return; }
        var target = targetGO.GetComponent<IInteractable>();
        if (target == null) { return; }
        
        bool success= target.Interact(interactionType, this);
        playerReceiver.interactionSuccessful = success;
        if (success)
        {
            OnInteraction?.Invoke();
        }
        else
        {
            OnFailedInteraction?.Invoke();
        }
        

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
