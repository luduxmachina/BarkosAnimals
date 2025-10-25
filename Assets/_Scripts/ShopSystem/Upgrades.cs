using System.Collections.Generic;
using UnityEngine;

public class Upgrades 
{
    
  
    public static List<PlayerEffect> playerUpgrades = new List<PlayerEffect>();

    public static void ClearUpgrades()
    {
        playerUpgrades.Clear();
    }
    public static void AddUpgrade(PlayerEffect upgrade)
    {
        playerUpgrades.Add(upgrade);
    }
   
}
