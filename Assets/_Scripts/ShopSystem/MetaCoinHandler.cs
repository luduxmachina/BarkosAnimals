using System.Collections.Generic;
using UnityEngine;

public class MetaCoinHandler : MonoBehaviour
{
    public static int metaCoinCount = 0;

    public static void AddMetaCoins(int amount)
    {
        metaCoinCount += amount;
        SaveToFile();
    }
    public static bool SpendMetaCoins(int amount)
    {
        if (metaCoinCount >= amount)
        {
            metaCoinCount -= amount;
            SaveToFile();
            return true;
        }
        return false;
    }
    [RuntimeInitializeOnLoadMethod]
    public static void LoadFromFile()
    {
        metaCoinCount = PlayerPrefs.GetInt("MetaCoinCount", 0);
    }
    public static void SaveToFile() //supongo que en las cookies se podra guardar algo
    {
        PlayerPrefs.SetInt("MetaCoinCount", metaCoinCount);
        PlayerPrefs.Save();
    }
}
