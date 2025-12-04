using UnityEngine;

public class CurrentSkin : MonoBehaviour
{
    private static SkinSO _currentDuckSkin;
    public static SkinSO currentDuckSkin {  get { return _currentDuckSkin; }

         set {
            _currentDuckSkin = value;
            SaveCurrentSkin();
        }
    }
    private static SkinSO _currentPlayerSkin;
    public static SkinSO currentPlayerSkin
    {
        get { return _currentPlayerSkin; }

        set
        {
            _currentPlayerSkin = value;
            SaveCurrentSkin();
        }
    }
    [RuntimeInitializeOnLoadMethod]
    public static void InitializeCurrentSkin()
    {
        
        string currentDuckSkinName = PlayerPrefs.GetString("CurrentDuckSkin", "Pato amarillo");
        string currentPlayerSkinName = PlayerPrefs.GetString("CurrentPlayerSkin", "Player default");
        SkinSO[] skin = Resources.LoadAll<SkinSO>("Skins");
        foreach (SkinSO s in skin)
        {
            if (s.skinName == currentDuckSkinName)
            {
                currentDuckSkin = s;
               // return;
            }
            if (s.skinName == currentPlayerSkinName)
            {
                currentPlayerSkin = s;
               // return;
            }
        }
        if(currentDuckSkin==null) currentDuckSkin = skin[0]; // Default skin if not found
        if (currentPlayerSkin == null) currentPlayerSkin = skin[0]; // Default skin if not found



    }
    private static void SaveCurrentSkin()
    {
        PlayerPrefs.SetString("CurrentDuckSkin", currentDuckSkin.skinName);
        PlayerPrefs.SetString("CurrentPlayerSkin", currentPlayerSkin.skinName);
        PlayerPrefs.Save();
    }

}
