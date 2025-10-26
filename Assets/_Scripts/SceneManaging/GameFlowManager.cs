#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;
    public QuotaChecker quotaChecker;
    public NextPhaseHandler nextPhaseHandler;

#if UNITY_EDITOR
    [SerializeField]
    private SceneAsset mainMenuScene;
#endif
    [SerializeField, HideInInspector]
    private int mainMenuSceneIndex = 0;

    [Header("Levels to play")]
    [SerializeField]
    private NivelSO defaultLevel;
    [SerializeField]
    private NivelSO[] tutorialLevels;
    [SerializeField]
    private NivelSO[] otherLevels;

    [Header("Settings")]
    public bool infiniteMode = false;

    [Header("--------")]
    public NivelSO currentLevel;
    public int currentLevelIndex = 0;

    [Header("CurrentLvl Info")]
    public LevelPhases currentPhase;
    public int currentArchipelago = 0;
    public int lastSelectedIsland = 0;
    public bool generatedIsland = false;
    public IslandSO currentIsland;

    private NivelSO[] levelsPlaying;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void Lose() 
    {
        GoToMainMenu();
    }
    public void Win()
    {
        GoToMainMenu();
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
        if (nextPhaseHandler.currentPhase == LevelPhases.QuotaPhase) //estamos en escena de quota
        {
            if (currentArchipelago >= currentLevel.numberOfArchipelagos) //fin de un nivel
            {
                EndOfLevelHandler();
                return;
            }
        }

        nextPhaseHandler.NextPhase();

        if (nextPhaseHandler.currentPhase == LevelPhases.QuotaPhase) //veniamos del barco y ahora estamos en escena de quota
        {
            EndOfIslandHandler();
            return;
        }

        LoadPlayingScene(currentLevel, currentArchipelago, currentPhase, lastSelectedIsland);

    }

    private void EndOfIslandHandler() //se llama a esto cuando es el final de una isla
    {
        if (currentArchipelago < currentLevel.numberOfArchipelagos) //seguimos en el mismo nivel, hay que pasar a siguiente isla
        {
            currentArchipelago++;
            NextPhase(); //no hay fase de cuota, nos la saltamos y seguimos aprendiendo
        }
    }
    private void EndOfLevelHandler()
    {
        if (!CheckQuotaFlag()) //Se ha perdido
        {
            Lose();
            return;
        }
        //siguiente nivel
        currentLevelIndex++;
        StartLevel(levelsPlaying, currentLevelIndex);
        return;
    }
    private void StartGame()
    {
        Upgrades.ClearUpgrades();
        InGameCoindHandler.coinCount = 0;
        StartLevel(levelsPlaying, currentLevelIndex);
    }
    private void StartLevel(NivelSO[] niveles, int index)
    {
        if (!infiniteMode && index >= niveles.Length)
        {

            Debug.Log("No more levels to play GG");
            Win();
            return;

        }

        currentLevel = levelsPlaying[index];
        SetNewQuota();
        nextPhaseHandler.Initialize();
        currentArchipelago = 0;
        generatedIsland = currentLevel.useGeneratedIslands;

        LoadPlayingScene(currentLevel, currentArchipelago, currentPhase, 0);
    }
    private void LoadPlayingScene(NivelSO level, int archipelagoIndex, LevelPhases phase, int chosenIsland)
    {
        int sceneToLoad = -1;
        switch (phase)
        {
            case LevelPhases.SelectionPhase:
                Debug.Log("Selection Phase: ");
                Debug.Log(level.archipelagos[archipelagoIndex].numberOfIslands + " islas entre las que elegir");
                if (level.archipelagos[archipelagoIndex].numberOfIslands == 1)
                {
                    Debug.Log("Nada que elegir, siguiente fase");
                    lastSelectedIsland = 0;
                    NextPhase(); //nada que elegir, avancen
                    return;
                }
                if (level.useDefaultSelectionPhaseScene)
                {
                    sceneToLoad = defaultLevel.selectionPhaseSceneIndex;
                }
                else
                {
                    sceneToLoad = level.selectionPhaseSceneIndex;
                }
                break;
            case LevelPhases.IslandPhase:
                if (level.useGeneratedIslands)
                {

                    sceneToLoad = defaultLevel.archipelagos[0].islands[chosenIsland].islandSceneIndex;
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
                //ahora no es una escena independiente :(

                break;
            case LevelPhases.QuotaPhase:
                if (level.useDefaultQuotaScene)
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
    private bool CheckQuotaFlag()
    {
        return quotaChecker.IsQuotaPass();
    }
    private void SetNewQuota()
    {
        //avisar aqui al quotachecker 
        if (currentLevel.useAutomaticQuota)
        {
            quotaChecker.GenerateCuote(currentLevelIndex);
        }
        else
        {
            quotaChecker.GenerateCuote(currentLevel.quotaInfo);
        }

    }
    private void Awake()
    {
        instance = this;
        quotaChecker = new QuotaChecker();
        nextPhaseHandler = new NextPhaseHandler();
        DontDestroyOnLoad(this.gameObject);

    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (mainMenuScene != null)
        {
            int tempMainMenuSceneIndex = SceneAssetGetIndex.ForceGetIndexOf(AssetDatabase.GetAssetPath(mainMenuScene));
            if (tempMainMenuSceneIndex != mainMenuSceneIndex)
            {
                mainMenuSceneIndex = tempMainMenuSceneIndex;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
