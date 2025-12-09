using UnityEngine;

public class Chimeneahandler : MonoBehaviour
{
    [SerializeField]
    private float AvancePorPiezaCarbon = 10.0f;
    [SerializeField]
    private PhaseTimer phaseTimer;
    public void RecibirCarbon()
    {
        phaseTimer.ForwardTime(AvancePorPiezaCarbon);
    }
    private void Start()
    {
        if (phaseTimer == null)
        {
            phaseTimer =Object.FindFirstObjectByType<PhaseTimer>( FindObjectsInactive.Include);
        }
        if (phaseTimer == null)
        {
            Debug.LogWarning("No se ha asignado el PhaseTimer al ChimeneaHandler y no se ha encontrado ninguno en la escena.");
            Destroy(this.gameObject);
        }
    }
}
