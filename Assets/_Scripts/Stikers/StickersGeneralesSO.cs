using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StickersGenerales", menuName = "Scriptable Objects/StickersGenerales")]
public class StickersGeneralesSO : ScriptableObject
{
    [field: SerializeField]List<Sticker> stickerList = new List<Sticker> ();
}
