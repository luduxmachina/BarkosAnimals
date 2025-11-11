using UnityEngine;

public class CurrentSkin : MonoBehaviour
{
 public static SkinSO currentSkin;
    [RuntimeInitializeOnLoadMethod]
    public static void InitializeCurrentSkin()
    {
        SkinSO[] allSkins = Resources.LoadAll<SkinSO>("Skins");
        if(allSkins.Length > 0)
        {
            currentSkin = allSkins[0];
        }
    }

}
