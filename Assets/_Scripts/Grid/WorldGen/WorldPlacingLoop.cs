using UnityEngine;

[RequireComponent(typeof(BWorldGridInput))]
public class WorldPlacingLoop : MonoBehaviour
{
    [SerializeField]
    public const int N_B_OBJECTS = 5;
    [SerializeField]
    public const int N_C_OBJECTS = 10;
    // public const int N_LOOPS = 5;

    private BWorldGridInput bGridInput;

    private void Awake()
    {
        bGridInput = GetComponent<BWorldGridInput>();
    }

    public void StartPlacing()
    {
        for (int i = 0; i < N_B_OBJECTS; i++)
        {
            bGridInput.PlaceObjOfID(2);
        }

        for (int i = 0; i < N_C_OBJECTS; i++)
        {
            // Grid C
        }
    }
}
