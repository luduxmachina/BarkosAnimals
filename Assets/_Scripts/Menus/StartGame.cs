using UnityEngine;

public class StartGame : MonoBehaviour
{

    public void StartGameCall()
    {
        GameFlowManager.instance.GoToGameSelection();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
