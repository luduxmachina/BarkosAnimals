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
        //para debug rapido
        TryGrab();
        //
        if (!detector.HasTarget() && !hasObjInHand) { return; } //no ocurre nada


    }
    public void TryGrab()
    {
        if(hasObjInHand) { 
            //soltar objeto
            objInHand.Drop();
           
            hasObjInHand = false;
            objInHand = null;
            return; 
        }

        if (!detector.HasTarget()) { return; } //no ocurre nada

        GameObject target = detector.GetTarget();
        Grabable grabable = target.GetComponent<Grabable>();
        if (grabable != null)
        {
            if (grabable.Grab(posCogerObj))
            {
                objInHand = grabable;
                hasObjInHand = true;
            }
        }
    }
}
