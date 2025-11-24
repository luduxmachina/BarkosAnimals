using System;
using UnityEngine;
using UnityEngine.Events;

public enum LevelPhases
{
    IslandPhase,
    OrganizationPhase,
    BoatPhase,
    QuotaPhase,
    End
}

[Serializable]
public class NextPhaseHandler
{
    [SerializeField]
    public LevelPhases currentPhase;

    [HideInInspector]
    public UnityEvent onStartIslandPhase = new();

    [HideInInspector]
    public UnityEvent onStartOrganizationPhase = new();

    [HideInInspector]
    public UnityEvent onStartBoatPhase = new();
    [HideInInspector]
    public UnityEvent onStartQuotaPhase = new();

    public void Initialize()
    {

        currentPhase = LevelPhases.IslandPhase;
    }
    public void NextPhase()
    {
        currentPhase++;

        CallPhaseEvent();
    }
    void CallPhaseEvent()
    {
        switch (currentPhase)
        {
            case LevelPhases.IslandPhase:
                onStartIslandPhase.Invoke();
                break;
            case LevelPhases.OrganizationPhase:
                onStartOrganizationPhase.Invoke();
                break;
            case LevelPhases.BoatPhase:
                onStartBoatPhase.Invoke();
                break;
            case LevelPhases.QuotaPhase:
                onStartQuotaPhase.Invoke();
                break;
        }
    }
    public UnityEvent GetPhaseEvent(LevelPhases phase)
    {
        return phase switch
        {

            LevelPhases.IslandPhase => onStartIslandPhase,
            LevelPhases.OrganizationPhase => onStartOrganizationPhase,
            LevelPhases.BoatPhase => onStartBoatPhase,
            LevelPhases.QuotaPhase => onStartQuotaPhase,
            _ => null,
        };
    }

}
