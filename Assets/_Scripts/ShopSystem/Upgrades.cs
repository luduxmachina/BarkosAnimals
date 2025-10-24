using System.Collections.Generic;
using UnityEngine;

public class Upgrades 
{
  
    public static List<PlayerEffect> playerUpgrades = new List<PlayerEffect>();

    public static void ClearUpgrades()
    {
        playerUpgrades.Clear();
    }
}
