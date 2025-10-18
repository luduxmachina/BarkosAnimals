using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartGameCallTutorial()
    {
        GameFlowManager.instance.StartTutorialGame();
    }

    public void StartGameCall()
    {
        GameFlowManager.instance.StartNormalGame();
    }
}
