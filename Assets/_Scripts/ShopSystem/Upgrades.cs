using System.Collections.Generic;

public class Upgrades 
{
    
  
    public static List<PlayerEffect> playerUpgrades = new List<PlayerEffect>();
    public static List<BoatUpgrades> boatUpgrades = new List<BoatUpgrades>();


    public static void ClearUpgrades()
    {
        playerUpgrades.Clear();
        boatUpgrades.Clear();
    }
    public static void AddUpgrade(PlayerEffect upgrade)
    {
        playerUpgrades.Add(upgrade);
    }
    public static void AddUpgrade(BoatUpgrades upgrade)
    {
        boatUpgrades.Add(upgrade);
    }

}
public enum BoatUpgrades
{
    SpeedBoost,
    FuelEfficiency,
    CargoCapacity
}