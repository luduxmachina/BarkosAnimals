using UnityEngine;

public class InGameCoindHandler 
{
    public static int coinCount = 0;

    public static void AddCoins(int amount)
    {
        coinCount += amount;
   
    }
    public static bool SpendCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
  
            return true;
        }
        return false;
    }
   
}
