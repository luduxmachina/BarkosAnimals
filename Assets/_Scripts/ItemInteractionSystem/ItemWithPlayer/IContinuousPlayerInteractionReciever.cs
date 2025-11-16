using UnityEngine;

public interface IContinuousPlayerInteractionReciever 
{

    bool OnPlayerStartInteraction(GameObject playerReference);
    void OnPlayerStopInteraction(GameObject playerReference);
}
