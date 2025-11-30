using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    
    public void PlayTutorial()
    {
        GameFlowManager.instance.StartTutorialGame();
    }
    public void PlayNormalLevel()
    {
        GameFlowManager.instance.StartNormalGame();
    }
    public void PlayChallengeLevel()
    {
        GameFlowManager.instance.StartChallengeGame();
    }
    public void GoBackToMainMenu()
    {
        GameFlowManager.instance.GoToMainMenu();
    }
}
