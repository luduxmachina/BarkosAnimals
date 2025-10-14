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
public enum GameModes
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
    public GameModes gameMode = GameModes.IOSBQ;

    [Header("--------")]
    public NivelSO currentLevel;
    public int currentLevelIndex = 0;

    [Header("CurrentLvl Info")]
    public QuotaInfo currentQuotaInfo;
    public LevelPhases currentPhase;
    public int currentArchipelago = 0;
    public int lastSelectedIsland = 0;

    private NivelSO[] levelsPlaying;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

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
                if (level.archipelagos[currentArchipelago].islands.Length == 1)
                {
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
    public void NextPhase()
    {
        if (gameMode == GameModes.IOSBQ){
            OtherNextPhase();
            return;
        }


        if (currentPhase == LevelPhases.QuotaPhase) //fin de una isla 
        {
            currentArchipelago++;
            if (currentArchipelago >= currentLevel.numberOfArchipelagos) //fin de un nivel
            {

                if (!CheckQuotaFlag()) //Se ha perdido
                {
                    Perder();
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
    private void OtherNextPhase()
    {
        //TODO: hacer lo mismo pero ocn otro orden de fases
        if (currentPhase == LevelPhases.QuotaPhase) //fin de una isla 
        {
            currentArchipelago++;
            if (currentArchipelago >= currentLevel.numberOfArchipelagos) //fin de un nivel
            {

                if (!CheckQuotaFlag()) //Se ha perdido
                {
                    Perder();
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

    public void Perder() //no se quien quiera hacer perder al player
    {
        //ir al menu principal
    }
}
