using UnityEngine;

public class CurrentSkin : MonoBehaviour
{
    private static SkinSO _currentSkin;
    public static SkinSO currentSkin { get { return _currentSkin; }

        private set {
            _currentSkin = value;
            SaveCurrentSkin();
        }
    }
    [RuntimeInitializeOnLoadMethod]
    public static void InitializeCurrentSkin()
    {
        
        string currentSkinName = PlayerPrefs.GetString("CurrentSkin", "Pato amarillo");
        SkinSO[] skin = Resources.LoadAll<SkinSO>("Skins");
        foreach (SkinSO s in skin)
        {
            if (s.skinName == currentSkinName)
            {
                currentSkin = s;
                return;
            }
        }
        currentSkin = skin[0]; // Default skin if not found

    }
    private static void SaveCurrentSkin()
    {
        PlayerPrefs.SetString("CurrentSkin", currentSkin.skinName);
        PlayerPrefs.Save();
    }

}
