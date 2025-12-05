using UnityEngine;

public class PlayerCurrentStats : MonoBehaviour
{
    [SerializeField]
    private PlayerBaseStats playerBaseStats;
    [ReadOnly]
    public Stats currentStats;

    [Header("Negative effects")]
    public bool canMove = true;
    public bool canJump = true;
    public bool canDash = true;
    

    public void ResetStats()
    {
        currentStats = playerBaseStats.baseStats.Clone();
        canDash = true;
        canJump = true;
        canMove = true;

    }
}
