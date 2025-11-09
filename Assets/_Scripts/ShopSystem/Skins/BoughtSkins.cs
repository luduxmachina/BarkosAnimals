using System.Collections.Generic;

public class BoughtSkins 
{
    public static List<string> boughtSkinsNames = new List<string>();

    public static void LoadFromFile() 
    {
        string data = ""; //cargarlo de algun sitio
        boughtSkinsNames = new List<string>(data.Split(','));
    }
    public static void SaveToFile() //supongo que en las cookies se podra guardar algo
    {
        string text= string.Join(",", boughtSkinsNames);
        //guardarlo en algun sitio

        return; 
    }
    public static bool IsSkinBought(string skinName)
    {
        return boughtSkinsNames.Contains(skinName);
    }
    public static void BuySkin(string skinName)
    {
        if (!boughtSkinsNames.Contains(skinName))
        {
            boughtSkinsNames.Add(skinName);
        }
        SaveToFile();
    }

}
