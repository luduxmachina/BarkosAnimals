using UnityEngine;
using UnityEngine.SceneManagement;
public enum LevelPhases
{
    IslandPhase,
    OrganizationPhase,
    BoatPhase,
    EndPhase
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

    [Header("--------")]
    public NivelSO currentLevel;
    public int currentLevelIndex = 0;

    [Header("CurrentLvl Info")]
    public QuotaInfo currentQuotaInfo;
    public LevelPhases currentPhase;
    public int currentIslandIndex = 0;

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

    private void StartGame()
    {

        currentPhase = LevelPhases.IslandPhase;

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
        currentPhase = LevelPhases.IslandPhase;
        currentIslandIndex = 0;
        LoadPlayingScene(currentLevel, currentIslandIndex, currentPhase);
    }
    private void LoadPlayingScene(NivelSO level, int islandIndex, LevelPhases phase)
    {
        if (islandIndex >= level.numberOfIslands)
        {
            Debug.Log("Ugggh???");
            return;
        }
        int sceneToLoad = -1;
        switch (phase)
        {
            case LevelPhases.IslandPhase:
                if (level.useDefaultIslands)
                {

                    sceneToLoad = defaultLevel.islandIndexes[0];
                }
                else
                {
                    sceneToLoad = level.islandIndexes[islandIndex];

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
            case LevelPhases.EndPhase:
                Debug.Log("Ugh???");
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
        if(currentPhase == LevelPhases.EndPhase) //fin de una isla 
        {
            if (!CheckQuotaFlag()) //Se ha perdido
            {
                Perder();
                return;
            }

            currentIslandIndex++;
            if (currentIslandIndex >= currentLevel.numberOfIslands) //fin de un nivel
            {
                currentLevelIndex++;
                StartLevel(levelsPlaying, currentLevelIndex);
                return;
            }
            else
            {
                currentPhase = LevelPhases.IslandPhase;
  
            }
        }
        else
        {
            currentPhase++;
        }
        LoadPlayingScene(currentLevel, currentIslandIndex, currentPhase);




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
