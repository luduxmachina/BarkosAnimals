using System;
using UnityEngine;

[Serializable]
public class PlayerEffect
{
    [SerializeField]
    public bool isPermanent=true;
  //  [HideIf("isPermanent")]
    public float duration;

    public virtual void ApplyEffect(PlayerCurrentStats playerMovement)
    {

    }
    /// <summary>
    /// Opcional
    /// </summary>
    /// <param name="playerMovement"></param>
    public virtual void RemoveEffect(PlayerCurrentStats playerMovement) 
    {

    }
}
