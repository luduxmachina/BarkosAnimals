using System.Collections.Generic;
using UnityEngine;

public class MetaCoinHandler : MonoBehaviour
{
    public static int metaCoinCount = 0;

    public static void AddMetaCoins(int amount)
    {
        metaCoinCount += amount;
    }
    public static bool SpendMetaCoins(int amount)
    {
        if (metaCoinCount >= amount)
        {
            metaCoinCount -= amount;
            return true;
        }
        return false;
    }
    public static void LoadFromFile()
    {
        string data = "0"; //cargarlo de algun sitio
        metaCoinCount = int.Parse(data);
    }
    public static void SaveToFile() //supongo que en las cookies se podra guardar algo
    {
        string text = metaCoinCount.ToString();
        return;
    }
}
