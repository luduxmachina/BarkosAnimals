using System.Collections.Generic;
using UnityEngine;

public class Upgrades 
{
    public static int inGameCoinCount = 0;
  
    public static List<PlayerEffect> playerUpgrades = new List<PlayerEffect>();

    public static void ClearUpgrades()
    {
        playerUpgrades.Clear();
    }
    public static void AddUpgrade(PlayerEffect upgrade)
    {
        playerUpgrades.Add(upgrade);
    }
    public static void AddMetaCoins(int amount)
    {
        inGameCoinCount += amount;
    }
    public static bool SpendMetaCoins(int amount)
    {
        if (inGameCoinCount >= amount)
        {
            inGameCoinCount -= amount;
            return true;
        }
        return false;
    }
}
