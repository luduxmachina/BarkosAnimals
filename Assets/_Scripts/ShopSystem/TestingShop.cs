using UnityEngine;

public class TestingShop : MonoBehaviour
{
   
    public void AddMetaCoin(int amount)
    {
        MetaCoinHandler.AddMetaCoins(amount);
    }
    public void SpendMetaCoin(int amount)
    {
        bool success = MetaCoinHandler.SpendMetaCoins(amount);
        if (success)
        {
            Debug.Log("Spent " + amount + " MetaCoins.");
        }
        else
        {
            Debug.Log("Not enough MetaCoins to spend " + amount + ".");
        }
    }
}
