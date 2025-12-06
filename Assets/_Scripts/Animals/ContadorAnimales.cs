using System.Collections.Generic;
using System;
using UnityEngine;

public class ContadorAnimales : MonoBehaviour
{
    public void checkCuoteWithOnlySecondFase()
    {
        GameFlowManager.instance.quotaChecker.ResetQuote();

        List<Stable> stables = Stable.allStables;

        foreach (Stable stable in stables)
        {
            foreach (ItemNames itemName in Enum.GetValues(typeof(ItemNames)))
            {
                int numAnim = stable.GetAnimalsInEstable(new ItemNames[] { itemName });
                GameFlowManager.instance.quotaChecker.UpdateCuote(new InventoryItemDataObjects(itemName, numAnim));
            }
            int animFelices = stable.GetHappyAnimals();
            GameFlowManager.instance.quotaChecker.UpdateQuoteWithHappinesOfAnimal(true, animFelices);

        }


    }
}
