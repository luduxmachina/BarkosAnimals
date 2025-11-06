using System.Collections.Generic;
using UnityEngine;

public class UpgradeEffects 
{
  
    public static List<PlayerEffect> effects = new List<PlayerEffect>();
    public static void ClearEffects()
    {
        effects.Clear();
    }
}
