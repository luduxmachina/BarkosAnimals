using UnityEngine;

[RequireComponent(typeof(WorldGridInput))]
public class WorldPlacingLoop : MonoBehaviour
{
    [SerializeField]
    private int N_B_OBJECTS = 5;
    [SerializeField]
    private int N_C_OBJECTS = 10;
    // public const int N_LOOPS = 5;

    private WorldGridInput gridInput;

    private void Awake()
    {
        gridInput = GetComponent<WorldGridInput>();
    }

    public void StartPlacing()
    {
        for (int i = 0; i < N_B_OBJECTS; i++)
        {
            gridInput.PlaceBObjOfRandomID();
        }

        for (int i = 0; i < N_C_OBJECTS; i++)
        {
            gridInput.PlaceCObjOfRandomID();
        }
    }
}
