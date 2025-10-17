using UnityEngine;
using UnityEngine.SceneManagement;
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
public class GameFlowManager : MonoBehaviour
{
    [Header("Levels to play")]
    [SerializeField]
    private NivelSO defaultLevel;
    [SerializeField]
    private NivelSO[] tutorialLevels;
    [SerializeField]
    private NivelSO[] otherLevels;

    [Header("Settings")]
    public bool infiniteMode = false;
    public GameModes gameMode = GameModes.SIOBQ;

    [Header("--------")]
    public NivelSO currentLevel;
    public int currentLevelIndex = 0;

    [Header("CurrentLvl Info")]
    public QuotaInfo currentQuotaInfo;
    public LevelPhases currentPhase;
    public int currentArchipelago = 0;
    public int lastSelectedIsland = 0;

    private NivelSO[] levelsPlaying;

    public void Lose() //no se quien quiera hacer perder al player
    {
        //ir al menu principal
    }
    public void Win()
    {
        // ir a algo de victoria
    }
    public void StartTutorialGame()
    {
        levelsPlaying = tutorialLevels;
        currentLevelIndex = 0;
        StartGame();
    }
    public void StartNormalGame()
    {
        levelsPlaying = otherLevels;
        currentLevelIndex = 0;
        StartGame();
    }
    public void StartGameAt(int levelIndex) //esto para los profes <3
    {
        levelsPlaying = otherLevels;
        currentLevelIndex = levelIndex;
        StartGame();
    }
    public void ChooseIsland(int island)
    {
        lastSelectedIsland = island;
    }
    public void NextPhase()
    {
        if(gameMode == GameModes.SIOBQ)
        {
            SIOBQNextPhase();

        }
        else
        {
            IOSBQNextPhase();
        }
    }

    private void StartGame()
    {

        StartLevel(levelsPlaying, currentLevelIndex);
    }
    private void StartLevel(NivelSO[] niveles, int index)
    {

        if (index >= niveles.Length)
        {
            if (infiniteMode)
            {
                currentLevelIndex = niveles.Length - 1;
                StartLevel(levelsPlaying, currentLevelIndex);
                return;
            }
            else //no deberia llegar aqui la vd
            {
                Debug.Log("No more levels to play GG");
                Win();
                return;
            }
        }
        currentLevel = levelsPlaying[index];

        if (gameMode == GameModes.SIOBQ)
            currentPhase = LevelPhases.SelectionPhase;
        else
            currentPhase = LevelPhases.IslandPhase;

        currentQuotaInfo = currentLevel.quotaInfo;
        currentArchipelago = 0;
        LoadPlayingScene(currentLevel, currentArchipelago, currentPhase, 0);
    }
    private void LoadPlayingScene(NivelSO level, int archipelagoIndex, LevelPhases phase, int chosenIsland)
    {
        if (archipelagoIndex >= level.numberOfArchipelagos)
        {
            Debug.Log("Ugggh???");
            return;
        }
        int sceneToLoad = -1;
        switch (phase)
        {
            case LevelPhases.SelectionPhase:
                Debug.Log("Selection Phase: ");
                Debug.Log(level.archipelagos[archipelagoIndex].numberOfIslands + " islas entre las que elegir");
                if (level.archipelagos[archipelagoIndex].numberOfIslands == 1)
                {
                    Debug.Log("Nada que elegir, siguiente ffase");
                    NextPhase(); //nada que elegir, avancen
                    return;
                }
                if(level.useDefaultSelectionPhaseScene)
                {
                    sceneToLoad = defaultLevel.selectionPhaseSceneIndex;
                }
                else
                {
                    sceneToLoad = level.selectionPhaseSceneIndex;
                }
                break;
            case LevelPhases.IslandPhase:
                if (level.useDefaultIslands)
                {

                    sceneToLoad = defaultLevel.archipelagos[0].islands[0].islandSceneIndex;
                }
                else
                {
                    sceneToLoad = level.archipelagos[archipelagoIndex].islands[chosenIsland].islandSceneIndex;

                }
                break;
            case LevelPhases.OrganizationPhase:
                if (level.useDefaultOrganizationPhaseScene)
                {
                    sceneToLoad = defaultLevel.organizationPhaseSceneIndex;
                }
                else
                {
                    sceneToLoad = level.organizationPhaseSceneIndex;
                }
                break;
            case LevelPhases.BoatPhase:
                if (level.useDefaultBoatPhaseScene)
                {
                    sceneToLoad = defaultLevel.boatPhaseSceneIndex;
                }
                else
                {
                    sceneToLoad = level.boatPhaseSceneIndex;
                }
                break;
            case LevelPhases.QuotaPhase:
                if(level.useDefaultQuotaScene)
                {
                    sceneToLoad = defaultLevel.quotaSceneIndex;
   
                }
                else
                {
                    sceneToLoad = level.quotaSceneIndex;
                }
                break;
        }
        if (sceneToLoad == -1)
        {
            Debug.Log("Scene to load not set correctly in the level data");
            return;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
    private void IOSBQNextPhase()
    {
     


        if (currentPhase == LevelPhases.QuotaPhase) //fin de una isla 
        {
            currentArchipelago++;
            if (currentArchipelago >= currentLevel.numberOfArchipelagos) //fin de un nivel
            {

                if (!CheckQuotaFlag()) //Se ha perdido
                {
                    Lose();
                    return;
                }

                currentLevelIndex++;
                StartLevel(levelsPlaying, currentLevelIndex);
                return;
            }
            else
            {
                currentPhase = LevelPhases.IslandPhase; //en esta version tras la quota viene isla directamente
  
            }
        }
        else
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
            }
        }
        LoadPlayingScene(currentLevel, currentArchipelago, currentPhase, lastSelectedIsland);




    }
    private void SIOBQNextPhase()
    {
        if (currentPhase == LevelPhases.QuotaPhase) //fin de una isla 
        {
            currentArchipelago++;
            if (currentArchipelago >= currentLevel.numberOfArchipelagos) //fin de un nivel
            {

                if (!CheckQuotaFlag()) //Se ha perdido
                {
                    Lose();
                    return;
                }

                currentLevelIndex++;
                StartLevel(levelsPlaying, currentLevelIndex);
                return;
            }
            else
            {
                currentPhase = LevelPhases.SelectionPhase;

            }
        }
        else
        {
            currentPhase++;
        }
        LoadPlayingScene(currentLevel, currentArchipelago, currentPhase, lastSelectedIsland);


    }
    private bool CheckQuotaFlag()
    {
        return true; //pasa la quota al siguiente nivel
    }


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }
}
