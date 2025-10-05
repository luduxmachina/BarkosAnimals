using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] TaggedDetector detector;
    [SerializeField] Transform posCogerObj;

    [SerializeField, ReadOnly] bool hasObjInHand = false;
    [SerializeField, ReadOnly, HideIf("hasObjInHand", false)]
    Grabable objInHand;
    public void Interact()
    {
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
            if (dropObj) { DropObj(); }

            return interacted;
        }
        return false;
    }
    public void GrabAction()
    {
        if(hasObjInHand) { 
            DropObj();
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
            objInHand = grabable;
            hasObjInHand = true;
            return; //se ha cogido el objeto
        }
    }
    public void DropObj() //lo dejo public porque algo puede hacerme soltar el objeto
    {
        if (!hasObjInHand) { return; } 
        //soltar objeto
        objInHand.Drop();

        hasObjInHand = false;
        objInHand = null;
        return;
    }
  
}
