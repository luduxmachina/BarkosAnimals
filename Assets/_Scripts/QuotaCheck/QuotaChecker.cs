using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class QuotaChecker
{
    Quota quota;

    Quota quotaPassed = new Quota();

    Dictionary<ItemNames, int> animalValues = new Dictionary<ItemNames, int>()
    {
        {ItemNames.Duck, 50 }, {ItemNames.Snake, 100}
    };

    List<ItemNames> sonAnimales = new List<ItemNames>
    {
        ItemNames.Snake,
        ItemNames.Duck
    };


    bool isQuotaPass = false;

    private List<QuotaUiInterface> quotaUIs = new List<QuotaUiInterface>();
    public void AddNewUI (QuotaUiInterface quotaUi)
    {
        quotaUIs.Add(quotaUi);
        quotaUi.UpdateQuotaPassed(quotaPassed, isQuotaPass);
    }


    /// <summary>
    /// This function set a new value for Quota
    /// </summary>
    /// <param name="quota"></param>
    /// <returns></returns>
    public Quota GenerateCuote(Quota quota)
    {
        this.quota = quota;
        return quota;
    }

    /// <summary>
    /// Create a new value for quota.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    /// 
    public Quota GenerateCuote(int level)
    {
        quota = new Quota(level);
        return quota;
    }

    /// <summary>
    /// Returns the value of the actual quota.
    /// </summary>
    /// <returns></returns>
    public Quota GetQuota() { return quota; }

    /// <summary>
    /// This function updates the state of the quote with the data provided. Is espected to receve a type of animal and the number of that animal obtained.
    /// </summary>
    /// <param name="animal"></param>
    /// <param name="number"></param>
    public void UpdateCuote(InventoryItemDataObjects animal)
    {
        ItemNames tipo = animal.Name;

        if(sonAnimales.Contains(tipo))
        {

            quotaPassed.AddPoints(animalValues[animal.Name]);

            this.isQuotaPass = this.quota.CheckIfPassed(quotaPassed);

            foreach (var item in quotaUIs)
            {
                item.UpdateQuotaPassed(quotaPassed, isQuotaPass);
            }

        }
    }

    public void checkCuoteWithOnlySecondFase(List<Stable>stables)
    {
        quotaPassed = new Quota();
        foreach (Stable stable in stables)
        {
            foreach(ItemNames itemName in Enum.GetValues(typeof(ItemNames)))
            {
                ItemNames[] name = {
                    itemName
                };
                int numAnim = stable.GetAnimalsInEstable(new ItemNames[] {itemName});
                this.UpdateCuote(new InventoryItemDataObjects(itemName, numAnim));
            }
        }
    }

    public void UpdateCuotaWithHappinesOfAnimal(bool anAnimalIsNowHappy)
    {
        if (anAnimalIsNowHappy)
        {
            this.quotaPassed.AddPoints(30);
        }
        else
        {
            this.quotaPassed.AddPoints(-30);
        }
    }

    /// <summary>
    /// This function returns if the quota is completed.
    /// </summary>
    /// <returns> 
    /// It returns a boolean that indicate if the quota is completed
    /// </returns>
    public bool IsQuotaPass()
    {
        return isQuotaPass;
    }
}
