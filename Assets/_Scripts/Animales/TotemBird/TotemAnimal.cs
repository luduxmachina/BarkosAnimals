using BehaviourAPI.Core;
using UnityEngine;

public class TotemAnimal : AAnimal
{
    [SerializeField]
    DetectorVision detectorVision;
    Transform lastSeenObjective;
    public override bool ObjectiveClose()
    {
        return base.ObjectiveClose();
    }
    public override bool PredatorClose()
    {
        return base.PredatorClose();
    }
    public override Transform GetClosestObjetive()
    {
        if (lastSeenObjective == null)
        {
            return lastSeenObjective;
        }
        else
        {
            return detectorVision.transformPan;
        }
    }
    public bool ObjectiveInSight()
    {
        bool objetivoVisto= detectorVision.hayPan;

        if (objetivoVisto)
        {
            lastSeenObjective = detectorVision.transformPan;

        }
        else
        {
            lastSeenObjective = null;
        }
        return objetivoVisto;
    }
    public bool PredatorInSight()
    {
        return detectorVision.hayPeligro;
    }
    public bool NotPredatorInSight()
    {
        return !PredatorInSight();
    }
    public void Cogido()
    {
        movimiento.CancelMove();
        lastSeenObjective = null;
        PlayIdleAnim();
    }
    public void CogerComida()
    {

    }

    public void LlevarComidaInit()
    {

    }
    public Status LlevarComidaUpdate()
    {
        return Status.Success;
    }

}
