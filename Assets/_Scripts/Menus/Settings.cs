using UnityEngine;

public class Settings
{
    private float volume;
    private bool isMuted;
    private int difficulty;

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
    private Settings()
    {
        volume = PlayerPrefs.GetFloat("volume", 0.5f);
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        difficulty = PlayerPrefs.GetInt("difficulty", 2);
    }
    public void NewVolume(float volume)
    {
        this.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void ToggleMute(bool isMuted)
    {
        this.isMuted = isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.Save();
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
}
