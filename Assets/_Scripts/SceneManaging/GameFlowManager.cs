#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
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
    public static GameFlowManager instance;
    public QuotaChecker quotaChecker;

#if UNITY_EDITOR
    public SceneAsset mainMenuScene;
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
    public GameModes gameMode = GameModes.SIOBQ;

    [Header("--------")]
    public NivelSO currentLevel;
    public int currentLevelIndex = 0;

    [Header("CurrentLvl Info")]
    public LevelPhases currentPhase;
    public int currentArchipelago = 0;
    public int lastSelectedIsland = 0;

    [HideInInspector]
    public UnityEvent onStartSelectionPhase= new();

    [HideInInspector]
    public UnityEvent onStartIslandPhase = new();

    [HideInInspector]
    public UnityEvent onStartOrganizationPhase = new();

    [HideInInspector]
    public UnityEvent onStartBoatPhase = new();

    [HideInInspector]
    public UnityEvent onStartQuotaPhase = new();


    private NivelSO[] levelsPlaying;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void Lose() //no se quien quiera hacer perder al player
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
        if(gameMode == GameModes.SIOBQ)
        {
            SIOBQNextPhase();

        }
        else
        {
            IOSBQNextPhase();
        }
        CallPhaseEvent();
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
        SetNewQuota();



        if (gameMode == GameModes.SIOBQ)
            currentPhase = LevelPhases.SelectionPhase;
        else
            currentPhase = LevelPhases.IslandPhase;

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
                /* //Se ve que ahora boat phase es la misma
                if (level.useDefaultBoatPhaseScene)
                {
                    sceneToLoad = defaultLevel.boatPhaseSceneIndex;
                }
                else
                {
                    sceneToLoad = level.boatPhaseSceneIndex;
                }*/

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
