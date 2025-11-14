using UnityEngine;

public class SimpleGrabber : MonoBehaviour, IGrabber
{

    private bool hasObjInHand = false;
    private IGrabbable objInHand;
    public void TryGrab(Transform objective)
    {
        IGrabbable grabbable = objective.GetComponentInParent<IGrabbable>();
        if (grabbable != null)
        {
            GrabObject(grabbable);
        }
    }
    public void StopGrabbing()
    {
        hasObjInHand = false;
        objInHand = null;
    }
    public void DropObj()
    {
        if (!hasObjInHand) { return; }

        //soltar objeto
        objInHand.Drop(); //esto, si puedo soltarlo, me llamaran a StopGrabbing

        return;
    }

    public void GrabObject(IGrabbable grabbable)
    {
        if (hasObjInHand) { DropObj(); return; } //si ya tengo algo en la mano, lo suelto primero

        if (grabbable.Grab(this.transform, this))
        {

            objInHand = grabbable;
            hasObjInHand = true;
            return; //se ha cogido el objeto
        }
    }
}
