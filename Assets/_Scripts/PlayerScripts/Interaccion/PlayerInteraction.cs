using UnityEngine;
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] TaggedDetector detector;
    [SerializeField] Transform posCogerObj;

    [SerializeField, ReadOnly] bool hasObjInHand = false;
    [SerializeField, ReadOnly, HideIf("hasObjInHand", false)]
    Grabable objInHand;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void Interact()
    {
        Debug.Log("Interacting");
        bool interacted = false;
        if (!hasObjInHand )
        {
            if (detector.HasTarget())
            {
               interacted= InteractWith(detector.GetTarget()); 
            }
        }
        else
        {
            Debug.Log("Interacting with obj in hand");
            interacted = InteractWith(objInHand.gameObject);

        }

        //si quereis hacer un sonidito o algo con interacted aqui:


    }
    private bool InteractWith(GameObject target)
    {
        IPlayerInteractionReciever interactable = target.GetComponent<IPlayerInteractionReciever>();
        if (interactable != null)
        {
            bool dropObj;
            bool interacted = interactable.OnPlayerInteraction(out dropObj);
            if (dropObj) { 
                Debug.Log("Dropping obj because interaction said so");
                DropObj(); }

            return interacted;
        }
        return false;
    }
    public void Grab()
    {
        if(hasObjInHand) { 
            DropObj();
            return;
        }

        if (!detector.HasTarget()) { return; } //no ocurre nada

        GameObject target = detector.GetTarget();
        Grabable grabable = target.GetComponent<Grabable>();
        if (grabable != null)
        {
            GrabObj(grabable);
            return; //se ha cogido el objeto
        }
        return; //no se ha cogido el objeto
    }
    private void GrabObj(Grabable grabable)
    {

        if (grabable.Grab(posCogerObj))
        {
            if (!grabable.allowExtramoveSetWhenGrabbed)
            {
                playerMovement.ImpedeExtraMoveset();
            }
            objInHand = grabable;
            hasObjInHand = true;
            return; //se ha cogido el objeto
        }
    }
    public void DropObj() //lo dejo public porque algo puede hacerme soltar el objeto
    {
        if (!hasObjInHand) { return; }
        playerMovement.AllowExtraMoveSet();
        //soltar objeto
        objInHand.Drop();

        hasObjInHand = false;
        objInHand = null;
        return;
    }
  
}
