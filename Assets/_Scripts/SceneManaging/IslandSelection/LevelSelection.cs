using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public ASyncLoader asyncLoader;
    
    public void PlayTutorial()
    {
        int scene = GameFlowManager.instance.StartTutorialGame();
        asyncLoader.LoadScene(scene);
    }
    public void PlayNormalLevel()
    {
        int scene = GameFlowManager.instance.StartNormalGame();
        asyncLoader.LoadScene(scene);
    }
    public void PlayChallengeLevel()
    {
        int scene = GameFlowManager.instance.StartChallengeGame();
        asyncLoader.LoadScene(scene);
    }
    public void GoBackToMainMenu()
    {
        GameFlowManager.instance.GoToMainMenu();
    }
}
