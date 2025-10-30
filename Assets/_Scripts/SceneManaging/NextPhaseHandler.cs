using System;
using UnityEngine;
using UnityEngine.Events;

public enum LevelPhases
{
    SelectionPhase,
    IslandPhase,
    OrganizationPhase,
    BoatPhase,
    QuotaPhase
}

public enum GameModes //esto es criminal y está hecho con spaghetti, todo lo que esté relacionado con esto 
{
    SIOBQ,
    IOSBQ
}
[Serializable]
public class NextPhaseHandler
{
    [SerializeField]
    GameModes gameMode;
    [SerializeField]
    public LevelPhases currentPhase;

    private GameFlowManager gameFlowManager => GameFlowManager.instance;

    [HideInInspector]
    public UnityEvent onStartSelectionPhase = new();

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

        if (gameMode == GameModes.SIOBQ)
            currentPhase = LevelPhases.SelectionPhase;
        else
            currentPhase = LevelPhases.IslandPhase;
    }
    public void NextPhase()
    {

        if (gameMode == GameModes.SIOBQ)
        {
            SIOBQNextPhase();
        }
        else
        {
            IOSBQNextPhase();
        }
        CallPhaseEvent();
    }
    private void IOSBQNextPhase()
    {
        switch (currentPhase)
        {
            case LevelPhases.IslandPhase:
                currentPhase = LevelPhases.OrganizationPhase;
                break;
            case LevelPhases.OrganizationPhase:
                currentPhase = LevelPhases.SelectionPhase;
                break;
            case LevelPhases.SelectionPhase:
                currentPhase = LevelPhases.BoatPhase;
                break;
            case LevelPhases.BoatPhase:
                currentPhase = LevelPhases.QuotaPhase;
                break;
            case LevelPhases.QuotaPhase:
                currentPhase = LevelPhases.IslandPhase;
                break;
        }

    }
    private void SIOBQNextPhase()
    {
        if (currentPhase == LevelPhases.QuotaPhase)
        {
            currentPhase = LevelPhases.SelectionPhase;
        }
        else
        {
            currentPhase++;
        }
    }
    void CallPhaseEvent()
    {
        switch (currentPhase)
        {
            case LevelPhases.SelectionPhase:
                onStartSelectionPhase.Invoke();
                break;
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

}
