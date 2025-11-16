using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StickersGenerales", menuName = "Scriptable Objects/StickersGenerales")]
public class StickersGeneralesSO : ScriptableObject
{
    public List<Sticker> stickerList = new();

    public Dictionary<StikersGenerales, Sprite> stickerDictionary = new();

    private void OnValidate()
    {
        foreach (var sticker in stickerList)
        {
            if (stickerDictionary.ContainsKey(sticker.tipo)) continue;
            stickerDictionary.Add(sticker.tipo, sticker.spritePegatina);
        }
    }


    public Sprite GetSticker(StikersGenerales stickerType)
    {
        return stickerDictionary[stickerType];
    }
}
