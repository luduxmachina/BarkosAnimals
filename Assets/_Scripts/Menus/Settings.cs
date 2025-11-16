using UnityEngine;

public class Settings
{
    private float volume = 0.5f;
    private bool isMuted = false;
    private int difficulty = 2; // 1: Easy, 2: Medium, 3: Hard

    private static Settings _instance;

    public static Settings Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Settings();
            return _instance;
        }
    }

    public void NewVolume(float volume)
    {
        this.volume = volume;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void ToggleMute(bool isMuted)
    {
        this.isMuted = isMuted;
    }
    public float GetVolume()
    {
        return volume;
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
}
