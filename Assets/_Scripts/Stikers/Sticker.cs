using System;
using UnityEngine;
using UnityEngine.UI;

public enum StikersGenerales
{
    None,
    Enfadado,
    Enfermo,
    Corazones,
    Incomodo,
    Cansancio,
    NecesitaLimpiar,
    NecesitaComerZanahoria,
    NecesitaComerCarne,
    NecesitaComerPan,
}


[Serializable]
public class Sticker
{
    [field: SerializeField]public StikersGenerales tipo;
    [field: SerializeField]public Sprite spritePegatina;
}
