using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class ContadorAnimales : MonoBehaviour
{

    [SerializeField]UnityEvent OnNoAnimals = new();
    bool animalesColocados = false;

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

    public void AnimalesColocados()
    {
        animalesColocados = true;
    }

    public void Update()
    {
        if (animalesColocados)
        {
            List<Stable> stables = Stable.allStables;

            int numAnim = 0;

            foreach (Stable stable in stables)
            {
                numAnim += stable.GetAnimalsInEstable();
            }

            if (numAnim <= 0)
            {
                OnNoAnimals.Invoke();
            }
        }
    }
}
