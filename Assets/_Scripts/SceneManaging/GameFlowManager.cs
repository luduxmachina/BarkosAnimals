#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;
    public QuotaChecker quotaChecker;

#if UNITY_EDITOR
    [SerializeField]
    private SceneAsset mainMenuScene;
    [SerializeField]
    private SceneAsset gameMenuScene;
#endif
    [SerializeField, HideInInspector]
    private int mainMenuSceneIndex = 0;
    [SerializeField, HideInInspector]
    private int gameMenuSceneIndex = 1;

    [Header("Levels to play")]
    [SerializeField]
    public NivelSO normalLevel;
    [SerializeField]
    private NivelSO tutorialLevel;
    [SerializeField]
    private NivelSO challengeLevel;


    [Header("--------")]
    [ReadOnly]
    public NivelSO currentLevel;

    [Header("CurrentLvl Info")]
   // public bool generatedIsland = false;
   [ReadOnly]
    public IslandSO currentIsland;
    [ReadOnly]
    public NextPhaseHandler nextPhaseHandler;


    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
    public void GoToGameSelection()
    {
        SceneManager.LoadScene(gameMenuSceneIndex);
    }
    public void StartTutorialGame()
    {
        currentLevel = tutorialLevel;
        StartGame();
    }
    public void StartNormalGame()
    {
        currentLevel = normalLevel;
        StartGame();
    }
    public void StartChallengeGame()
    {
        currentLevel = challengeLevel;
        StartGame();
    }
    public void NextPhase()
    {
        nextPhaseHandler.NextPhase();
      
        LoadPlayingScene(currentLevel, nextPhaseHandler.currentPhase);

    }

    
    private void StartGame()
    {
        Upgrades.ClearUpgrades();
        InGameCoindHandler.coinCount = 0;
        StartLevel();
    }
    private void StartLevel()
    {
       
        SetNewQuota();
        nextPhaseHandler.Initialize();
        currentIsland= currentLevel.archipelagos[0].islands[0]; //ya no hay nada que hacer aqui

        LoadPlayingScene(currentLevel, nextPhaseHandler.currentPhase);
    }
    private void LoadPlayingScene(NivelSO level,LevelPhases phase)
    {
        int sceneToLoad = -1;
        switch (phase)
        {
           
            case LevelPhases.IslandPhase:

                sceneToLoad = level.archipelagos[0].islands[0].islandSceneIndex;

                
                break;
            case LevelPhases.OrganizationPhase:

                 sceneToLoad = level.organizationPhaseSceneIndex;
               
                break;
            case LevelPhases.BoatPhase:
                //Esto no carga escena

                break;
            case LevelPhases.QuotaPhase:
                sceneToLoad = level.quotaSceneIndex;
                
                break;
            case LevelPhases.End:
                GoToMainMenu();
                return;
        }
        if (sceneToLoad == -1)
        {

            return;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
    private void SetNewQuota()
    {
        //ya pierde sentido

       quotaChecker.GenerateCuote(0);


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
            int tempGameMenuSceneIndex = SceneAssetGetIndex.ForceGetIndexOf(AssetDatabase.GetAssetPath(gameMenuScene));
            if (tempGameMenuSceneIndex != gameMenuSceneIndex)
            {
                gameMenuSceneIndex = tempGameMenuSceneIndex;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
