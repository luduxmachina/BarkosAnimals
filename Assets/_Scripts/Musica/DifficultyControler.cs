using UnityEngine;

public class DifficultyControler : MonoBehaviour
{
    public void ChangeDifficultyLevel(float difficulty)
    {
        Settings.Instance.SetDifficulty((int)difficulty);
    }
}
