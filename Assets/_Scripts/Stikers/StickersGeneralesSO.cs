using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StickersGenerales", menuName = "Scriptable Objects/StickersGenerales")]
public class StickersGeneralesSO : ScriptableObject
{
    public List<Sticker> stickerList = new();

    public Dictionary<StikersGenerales, Sprite> stickerDictionary = new();

    bool inicializado;

    public void Inicializar()
    {
        foreach (var sticker in stickerList)
        {
            if (stickerDictionary.ContainsKey(sticker.tipo)) continue;
            stickerDictionary.Add(sticker.tipo, sticker.spritePegatina);
        }
        inicializado = true;
    }


    public Sprite GetSticker(StikersGenerales stickerType)
    {
        if(!inicializado)Inicializar();
        return stickerDictionary[stickerType];
    }
}
