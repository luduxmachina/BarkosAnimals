using System.Collections.Generic;
using UnityEngine;

public class BoughtSkins 
{
    public static List<string> boughtDuckSkinsNames = new List<string>();
    public static List<string> boughtPlayerSkinsNames = new List<string>();

    [RuntimeInitializeOnLoadMethod]
    public static void LoadFromFile() 
    {
        string data = PlayerPrefs.GetString("BoughtSkins", "Pato amarillo");
        boughtDuckSkinsNames = new List<string>(data.Split(','));
        data = PlayerPrefs.GetString("BoughtPlayerSkins", "Barkito");
        boughtPlayerSkinsNames = new List<string>(data.Split(','));
    }
    public static void SaveToFile() //supongo que en las cookies se podra guardar algo
    {
        string text= string.Join(",", boughtDuckSkinsNames);
        //guardarlo en algun sitio
        PlayerPrefs.SetString("BoughtDuckSkins", text);

        text = string.Join(",", boughtPlayerSkinsNames);
        PlayerPrefs.SetString("BoughtPlayerSkins", text);

        return; 
    }
    public static bool IsSkinBought(string skinName)
    {
        return boughtDuckSkinsNames.Contains(skinName) || boughtPlayerSkinsNames.Contains(skinName);
    }
    public static void BuySkin(string skinName, TipoSkin tipoSkin)
    {
        if(tipoSkin == TipoSkin.player)
        {
            if (!boughtPlayerSkinsNames.Contains(skinName))
            {
                boughtPlayerSkinsNames.Add(skinName);
            }
   
        }
        if(tipoSkin == TipoSkin.pato)
        {

            if (!boughtDuckSkinsNames.Contains(skinName))
            {
                boughtDuckSkinsNames.Add(skinName);
            }
      
        }
        SaveToFile();
    }

}
